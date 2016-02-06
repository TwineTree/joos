using Abp.Application.Services.Dto;
using Joos.JoosCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.JoosApp.Dto
{
    public class QuestionInput : IInputDto
    {
        public string QuestionText { get; set; }

        public string PositiveValue { get; set; }

        public string NegativeValue { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

    }
}
