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

        public VoteService(
            IRepository<Vote> voteRepository
            )
        {
            _voteRepository = voteRepository;
        }

        public bool Insert(VoteInput vote)
        {
            try
            {
                _voteRepository.Insert(new Vote
                {
                    QuestionId = vote.QuestionId,
                    Value = vote.Value
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
