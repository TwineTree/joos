using Abp.WebApi.Authorization;
using Abp.WebApi.Controllers;
using Abp.Web.Models;
using System.Web.Http;
using System.Net;
using System.Web.Http.Results;
using Joos.Users;
using System.Threading.Tasks;
using System.Threading;
using Abp.Runtime.Session;
using Joos.ExternalServices;
using Joos.JoosApp;
using Joos.Api.Models;
using Joos.JoosApp.Dto;

namespace Joos.Api.Controllers
{
    [AbpApiAuthorize]
    public class ApiTestController : AbpApiController
    {
        private IAbpSession _abpSession;
        private IFacebookService _facebookService;
        private IUserAppService _userAppService;
        private readonly UserManager _userManager;

        public ApiTestController(
            UserManager userManager,
            IFacebookService facebookService,
            IUserAppService userAppService,
            IAbpSession abpSession)
        {
            _userManager = userManager;
            _userAppService = userAppService;
            _abpSession = abpSession;
            _facebookService = facebookService;
        }

        // /api/apitest/test
        [HttpPost]
        public async Task<AjaxResponse> Test()
        {
            //var input = new QuestionsInput();
            //var questions = input.Questions;
            //questions.Question = "How you doing?";
            //questions.PositiveValue = "Good";
            //questions.NegativeValue = "Not Good";
            //questions.ImageUrl = "l";
            //questions.VideoUrl = "l";

            var user = await _userAppService.GetUserByIdAsync(10);
            //questions.User = user;

            return new AjaxResponse(user);
        }
    }
}
