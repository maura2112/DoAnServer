using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        public int UserId { get; set; }
        public int SkillId { get; set; }

        public virtual Skill? Skill { get; set; } = null!;

        public virtual AppUser? AppUser { get; set; } = null!;
    }
}
