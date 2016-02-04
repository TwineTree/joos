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

namespace Joos.Api.Controllers
{
    [AbpApiAuthorize]
    public class ApiTestController : AbpApiController
    {
        private IAbpSession _abpSession;
        private IFacebookService _facebookService;
        private IUserAppService _userAppService;

        public ApiTestController(
            IFacebookService facebookService,
            IUserAppService userAppService,
            IAbpSession abpSession)
        {
            _userAppService = userAppService;
            _abpSession = abpSession;
            _facebookService = facebookService;
        }

        // /api/apitest/test
        [HttpPost]
        public async Task<AjaxResponse> Test()
        {
            var uf = await _facebookService.GetUserProfile("CAAGXzBr94ZA4BAGhYitOQEsRQiiIf0tkVWY477pRbmNeXnKyQUDZCk9KbrijkoKjCUFJLKEOsIlHUrvqURjt1LZAewYojCWFTZA50SsUHsFfLJqBJCyrCYh13euxGJbt285jHRvIjQEl4WN5UkGvGNobWKe0nMMt4NQGDITkaxHz807ai636ajVv7d3TOd7URLz83zZALvwuVGdYoSjgF");
            return new AjaxResponse(uf);
        }

    }
}
