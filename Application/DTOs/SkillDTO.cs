using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SkillDTO : BaseEntity
    {
        public string SkillName { get; set; }
        public virtual ICollection<ProjectSkillDTO> ProjectSkills { get; set; }

        public virtual ICollection<UserSkillDTO> UserSkills { get; set; }
    }
}
