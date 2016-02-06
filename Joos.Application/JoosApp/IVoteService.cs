using Abp.Application.Services;
using Joos.JoosApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.JoosApp
{
    public interface IVoteService : IApplicationService
    {
        Task<bool> Insert(VoteInput vote);
    }
}
