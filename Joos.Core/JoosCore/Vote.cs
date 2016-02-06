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
    [Table("AbpVote")]
    public class Vote : FullAuditedEntity<int, User>
    {

        public virtual int? QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        public bool Value { get; set; }
    }
}
