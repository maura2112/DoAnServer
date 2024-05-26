using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectSkillDTO : BaseEntity
    {
        public int ProjectId { get; set; }
        public int SkillId { get; set; }

        public virtual SkillDTO? Skill { get; set; } = null!;

        public virtual ProjectDTO? Project { get; set; } = null!;
    }
}
