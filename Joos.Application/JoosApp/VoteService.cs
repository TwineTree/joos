using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joos.JoosApp.Dto;
using Abp.Domain.Repositories;
using Joos.JoosCore;

namespace Joos.JoosApp
{
    public class VoteService : IVoteService
    {
        private readonly IRepository<Vote> _voteRepository;
        private readonly IRepository<Question> _questionRepository;

        public VoteService(
            IRepository<Question> questionRepository,
            IRepository<Vote> voteRepository
            )
        {
            _questionRepository = questionRepository;
            _voteRepository = voteRepository;
        }

        public async Task<bool> Insert(VoteInput vote)
        {
            try
            {
                await _voteRepository.InsertAsync(new Vote
                {
                    QuestionId = vote.QuestionId,
                    Value = vote.Value
                });

                var query = _questionRepository.GetAll();
                var question = query.FirstOrDefault(q => q.Id.Equals(vote.QuestionId));
                var r = (vote.Value == true) ? question.PositiveVotes++ : question.NegativeVotes++;
                await _questionRepository.UpdateAsync(question);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
