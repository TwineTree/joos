using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Joos.JoosCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.JoosApp.Dto
{
    [AutoMapFrom(typeof(Vote))]
    public class VoteInput : EntityDto
    {
        public int QuestionId { get; set; }

        public bool Value { get; set; }
    }
}
