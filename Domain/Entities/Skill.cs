using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string SkillName { get; set; }

        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; } = null!;
    }
}
