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

namespace Joos.Api.Controllers
{
    [AbpApiAuthorize]
    public class ApiTestController : AbpApiController
    {
        private IAbpSession _abpSession;
        private IUserAppService _userAppService;

        public ApiTestController(IUserAppService userAppService,
            IAbpSession abpSession)
        {
            _userAppService = userAppService;
            _abpSession = abpSession;
        }

        // /api/apitest/test
        [HttpPost]
        public async Task<AjaxResponse> Test()
        {
            await _userAppService.TestTask(_abpSession.GetUserId());
            return new AjaxResponse(new { hello = "world" });
        }

    }
}
