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


        public virtual ICollection<ProjectSkill> ProjectSkills { get; set; }

        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
