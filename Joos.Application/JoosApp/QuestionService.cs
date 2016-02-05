using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joos.JoosCore;
using Abp.Domain.Repositories;
using Joos.JoosApp.Dto;
using Joos.Users;

namespace Joos.JoosApp
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionsRepository;
        private readonly UserManager _userManager;

        public QuestionService(
            UserManager userManager,
            IRepository<Question> questionsRepository
            )
        {
            _userManager = userManager;
            _questionsRepository = questionsRepository;
        }

        public bool Insert(QuestionsInput question)
        {
            try
            {
                _questionsRepository.Insert(new Question
                {
                    QuestionText = question.QuestionText,
                    PositiveValue = question.PositiveValue,
                    NegativeValue = question.NegativeValue,
                    ImageUrl = question.ImageUrl,
                    VideoUrl = question.VideoUrl
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
