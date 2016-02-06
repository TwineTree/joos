using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Joos.JoosCore;

namespace Joos.JoosApp.Dto
{
    [AutoMapFrom(typeof(Question))]
    public class QuestionInput : EntityDto
    {
        public string QuestionText { get; set; }

        public string PositiveValue { get; set; }

        public string NegativeValue { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public int PositiveVotes { get; set; }

        public int NegativeVotes { get; set; }
    }
}
