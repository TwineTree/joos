using System.Collections.Generic;
using System.Linq;
using Joos.JoosCore;
using Abp.Domain.Repositories;
using Joos.JoosApp.Dto;
using Joos.Users;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using Abp.Runtime.Caching;
using System;

namespace Joos.JoosApp
{
    public class QuestionService : IQuestionService
    {
        private const string QUESTION_TAG = "QUESTION";

        private readonly IRepository<Question> _questionsRepository;
        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;

        public QuestionService(
            ICacheManager cacheManager,
            UserManager userManager,
            IRepository<Question> questionsRepository
            )
        {
            _cacheManager = cacheManager;
            _userManager = userManager;
            _questionsRepository = questionsRepository;
        }

        public async Task<IEnumerable<QuestionInput>> GetQuestions(int pageIndex, int pageSize)
        {
            return await _cacheManager
                .GetCache(QUESTION_TAG)
                .GetAsync(QUESTION_TAG, () => GetFromDatabase(pageIndex, pageSize))
                as IEnumerable<QuestionInput>;

        }

        private async Task<IEnumerable<QuestionInput>> GetFromDatabase(int pageIndex, int pageSize)
        {
            var query = _questionsRepository.GetAll();

            var skip = pageIndex * pageSize;

            query = query.OrderByDescending(q => q.CreationTime);

            query = query.Skip(skip).Take(pageSize);

            var lis = await query.ToListAsync();

            return lis.Select(question => Mapper.Map<QuestionInput>(question));
        }

        public bool Insert(QuestionInput question)
        {
            try
            {
                _cacheManager.GetCache(QUESTION_TAG).Clear();
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
