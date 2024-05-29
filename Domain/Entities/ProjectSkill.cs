using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectSkill : BaseEntity
    {
        public int ProjectId { get; set; }
        public int SkillId { get; set;}

        public virtual Skill? Skill { get; set; } = null!;

        public virtual Project? Project { get; set; } = null!;
    }
}
