using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Authorization.Users;
using Abp.UI;
using Abp.Web.Models;
using Abp.WebApi.Controllers;
using Joos.Api.Models;
using Joos.Authorization.Roles;
using Joos.MultiTenancy;
using Joos.Users;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using Abp.Domain.Uow;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Joos.ExternalServices;
using System.Linq;
using Abp.Threading;
using Microsoft.AspNet.Identity.Owin;
using Abp.IdentityFramework;

namespace Joos.Api.Controllers
{
    public class AccountController : AbpApiController
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;

            }
        }

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private IFacebookService _facebookService;
        private IGoogleService _googleService;

        static AccountController()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }

        public AccountController(GoogleService googleService,
            RoleManager roleManager,
            UserManager userManager,
            IFacebookService facebookService,
            IUnitOfWorkManager unitOfWorkManager,
            TenantManager tenantManager)
        {
            _googleService = googleService;
            _roleManager = roleManager;
            _userManager = userManager;
            _facebookService = facebookService;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantManager = tenantManager;
        }

        //Phone number
        [HttpPost]
        public async Task<AjaxResponse> Authenticate(LoginModel loginModel)
        {
            CheckModelState();

            var loginResult = await GetLoginResultAsync(loginModel.UsernameOrEmailAddress, loginModel.Password, loginModel.TenancyName);

            var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

            return new AjaxResponse(OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
        }

        private async Task<AbpUserManager<Tenant, Role, User>.AbpLoginResult> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _userManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private Exception CreateExceptionForFailedLoginAttempt(AbpLoginResultType result, string usernameOrEmailAddress, string tenancyName)
        {
            switch (result)
            {
                case AbpLoginResultType.Success:
                    return new ApplicationException("Don't call this method with a success result!");
                case AbpLoginResultType.InvalidUserNameOrEmailAddress:
                case AbpLoginResultType.InvalidPassword:
                    return new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                case AbpLoginResultType.InvalidTenancyName:
                    return new UserFriendlyException(L("LoginFailed"), L("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                case AbpLoginResultType.TenantIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("TenantIsNotActive", tenancyName));
                case AbpLoginResultType.UserIsNotActive:
                    return new UserFriendlyException(L("LoginFailed"), L("UserIsNotActiveAndCanNotLogin", usernameOrEmailAddress));
                case AbpLoginResultType.UserEmailIsNotConfirmed:
                    return new UserFriendlyException(L("LoginFailed"), "Your email address is not confirmed. You can not login"); //TODO: localize message
                default: //Can not fall to default actually. But other result types can be added in the future and we may forget to handle it
                    Logger.Warn("Unhandled login fail reason: " + result);
                    return new UserFriendlyException(L("LoginFailed"));
            }
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException("Invalid request!");
            }
        }

        [UnitOfWork]
        protected virtual async Task<List<Tenant>> FindPossibleTenantsOfUserAsync(UserLoginInfo login)
        {
            List<User> allUsers;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                allUsers = await _userManager.FindAllAsync(login);
            }

            return allUsers
                .Where(u => u.TenantId != null)
                .Select(u => AsyncHelper.RunSync(() => _tenantManager.FindByIdAsync(u.TenantId.Value)))
                .ToList();
        }

        #region External Login

        [HttpPost]
        public async Task<AjaxResponse> ExternalLogin(string provider, string accessToken, string id = "", string tenancyName = "")
        {
            switch (provider)
            {
                case "Facebook":
                    return await facebookLogin(provider, accessToken, tenancyName);
                case "Google":
                    return await googleLogin(provider, accessToken, id, tenancyName);
                default:
                    break;
            }

            return new AjaxResponse(true);
        }

        #endregion

        #region Helpers
        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected async Task<AjaxResponse> facebookLogin(string provider, string accessToken, string tenancyName = "")
        {
            var uf = await _facebookService.GetUserProfile(accessToken);
            if (uf != null && uf.error == null)
            {
                var loginInfo = new ExternalLoginInfo
                {
                    DefaultUserName = uf.email,
                    Email = uf.email,
                    Login = new UserLoginInfo("Facebook", uf.id)
                };

                var tenantId = 0;
                // Search for user in DB
                if (string.IsNullOrWhiteSpace(tenancyName))
                {
                    var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                    switch (tenants.Count)
                    {
                        case 0:
                            //register
                            break;
                        case 1:
                            tenancyName = tenants[0].TenancyName;
                            tenantId = tenants[0].Id;
                            break;
                        default:
                            tenancyName = tenants[0].TenancyName;
                            tenantId = tenants[0].Id;
                            break;
                    }
                }

                var loginResult = await _userManager.LoginAsync(loginInfo.Login, tenancyName);

                switch (loginResult.Result)
                {
                    case AbpLoginResultType.Success:
                        var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

                        var currentUtc = new SystemClock().UtcNow;
                        ticket.Properties.IssuedUtc = currentUtc;
                        ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

                        var myAccessToken = OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

                        return new AjaxResponse(new { UserProfile = uf, MyAccessToken = myAccessToken });

                    case AbpLoginResultType.UnknownExternalLogin:
                        //register
                        var user = new User
                        {
                            Name = uf.name,
                            Surname = uf.last_name,
                            EmailAddress = uf.email,
                            IsActive = true
                        };

                        user.Logins = new List<UserLogin>
                                {
                                    new UserLogin
                                    {
                                        LoginProvider = "Facebook",
                                        ProviderKey = uf.id
                                    }
                                };
                        user.UserName = uf.email;
                        user.Password = new PasswordHasher().HashPassword(uf.id);

                        //Switch to the tenant
                        //_unitOfWorkManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant);
                        //_unitOfWorkManager.Current.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId);

                        //Add default roles
                        //user.Roles = new List<UserRole>();
                        //foreach (var defaultRole in _roleManager.Roles.Where(r => r.IsDefault).ToList())
                        //{
                        //    user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                        //}

                        //Save user
                        CheckErrors(await _userManager.CreateAsync(user));
                        //await _unitOfWorkManager.Current.SaveChangesAsync();

                        // Generataing token
                        loginResult = await _userManager.LoginAsync(loginInfo.Login, tenancyName);
                        ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

                        currentUtc = new SystemClock().UtcNow;
                        ticket.Properties.IssuedUtc = currentUtc;
                        ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

                        myAccessToken = OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

                        return new AjaxResponse(new { UserProfile = uf, MyAccessToken = myAccessToken });
                    default:
                        throw CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
                }

            }
            else
            {
                throw new UserFriendlyException(L("LoginFailed"), L("Facebook Login Failed"));
            }
        }

        protected async Task<AjaxResponse> googleLogin(string provider, string accessToken, string id = "", string tenancyName = "")
        {
            var uf = await _googleService.getUserProfile(accessToken, id);
            if (uf != null && uf.error == null)
            {
                var loginInfo = new ExternalLoginInfo
                {
                    DefaultUserName = uf.emails[0].value,
                    Email = uf.emails[0].value,
                    Login = new UserLoginInfo("Google", uf.id)
                };

                var tenantId = 0;
                // Search for user in DB
                if (string.IsNullOrWhiteSpace(tenancyName))
                {
                    var tenants = await FindPossibleTenantsOfUserAsync(loginInfo.Login);
                    switch (tenants.Count)
                    {
                        case 0:
                            //register
                            break;
                        case 1:
                            tenancyName = tenants[0].TenancyName;
                            tenantId = tenants[0].Id;
                            break;
                        default:
                            tenancyName = tenants[0].TenancyName;
                            tenantId = tenants[0].Id;
                            break;
                    }
                }

                var loginResult = await _userManager.LoginAsync(loginInfo.Login, tenancyName);

                switch (loginResult.Result)
                {
                    case AbpLoginResultType.Success:
                        var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

                        var currentUtc = new SystemClock().UtcNow;
                        ticket.Properties.IssuedUtc = currentUtc;
                        ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

                        var myAccessToken = OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

                        return new AjaxResponse(new { UserProfile = uf, MyAccessToken = myAccessToken });

                    case AbpLoginResultType.UnknownExternalLogin:
                        //register
                        var user = new User
                        {
                            Name = uf.displayName,
                            Surname = uf.name.familyName,
                            EmailAddress = uf.emails[0].value,
                            IsActive = true
                        };

                        user.Logins = new List<UserLogin>
                                {
                                    new UserLogin
                                    {
                                        LoginProvider = "Google",
                                        ProviderKey = uf.id
                                    }
                                };
                        user.UserName = uf.emails[0].value;
                        user.Password = new PasswordHasher().HashPassword(uf.id);

                        //Switch to the tenant
                        //_unitOfWorkManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant);
                        //_unitOfWorkManager.Current.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenantId);

                        //Add default roles
                        //user.Roles = new List<UserRole>();
                        //foreach (var defaultRole in _roleManager.Roles.Where(r => r.IsDefault).ToList())
                        //{
                        //    user.Roles.Add(new UserRole { RoleId = defaultRole.Id });
                        //}

                        //Save user
                        CheckErrors(await _userManager.CreateAsync(user));
                        //await _unitOfWorkManager.Current.SaveChangesAsync();

                        // Generataing token
                        loginResult = await _userManager.LoginAsync(loginInfo.Login, tenancyName);
                        ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

                        currentUtc = new SystemClock().UtcNow;
                        ticket.Properties.IssuedUtc = currentUtc;
                        ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

                        myAccessToken = OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

                        return new AjaxResponse(new { UserProfile = uf, MyAccessToken = myAccessToken });
                    default:
                        throw CreateExceptionForFailedLoginAttempt(loginResult.Result, loginInfo.Email ?? loginInfo.DefaultUserName, tenancyName);
                }

            }
            else
            {
                throw new UserFriendlyException(L("LoginFailed"), L("Facebook Login Failed"));
            }
        }
        #endregion
    }
}
