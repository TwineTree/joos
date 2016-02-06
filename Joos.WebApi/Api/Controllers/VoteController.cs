using Abp.Web.Models;
using Abp.WebApi.Authorization;
using Abp.WebApi.Controllers;
using Joos.Api.Models;
using Joos.JoosApp;
using Joos.JoosApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Joos.Api.Controllers
{
    [AbpApiAuthorize]
    public class VoteController : AbpApiController
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpPost]
        public async Task<AjaxResponse> Add([FromBody]VoteModel model)
        {
            var vote = new VoteInput();
            vote.QuestionId = model.QuestionId;
            vote.Value = model.Value;
            var result = await _voteService.Insert(vote);
            return new AjaxResponse(result);
        }
    }
}
