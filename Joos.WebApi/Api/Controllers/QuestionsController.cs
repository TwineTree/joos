using Abp.Web.Models;
using Abp.WebApi.Authorization;
using Abp.WebApi.Controllers;
using Joos.Api.Models;
using Joos.JoosApp;
using Joos.JoosApp.Dto;
using Joos.JoosCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Joos.Api.Controllers
{
    [AbpApiAuthorize]
    public class QuestionsController : AbpApiController
    {
        private IQuestionService _questionsService;
        
        public QuestionsController(IQuestionService questionsService)
        {
            _questionsService = questionsService;
        }

        // /api/Questions/Add
        [HttpPost]
        public AjaxResponse Add([FromBody]QuestionModel model)
        {
            var input = new QuestionInput();
            input.QuestionText = model.QuestionText;
            input.PositiveValue = model.PositiveValue;
            input.NegativeValue = model.NegativeValue;
            input.ImageUrl = model.ImageUrl;
            input.VideoUrl = model.VideoUrl;

            _questionsService.Insert(input);
            return new AjaxResponse(true);
        }
    }
}
