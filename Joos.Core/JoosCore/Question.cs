using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Joos.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.JoosCore
{
    [Table("AbpQuestions")]
    public class Question : FullAuditedEntity<int, User> 
    {
        public string QuestionText { get; set; }

        public string PositiveValue { get; set; }

        public string NegativeValue { get; set; }

        public string ImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public int PositiveVotes { get; set; }

        public int NegativeVotes { get; set; }

        [ForeignKey("QuestionId")]
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
