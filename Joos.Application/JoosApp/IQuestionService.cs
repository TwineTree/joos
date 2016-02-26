using Abp.Application.Services;
using Joos.JoosApp.Dto;
using Joos.JoosCore;
using Joos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.JoosApp
{
    public interface IQuestionService : IApplicationService
    {
        bool Insert(QuestionInput questions);

        Task<IEnumerable<QuestionInput>> GetQuestions(int pageIndex, int pageCount, int id);
    }
}
