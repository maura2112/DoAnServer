using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string SkillName { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("User")]
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Category? Category { get; set; } = null!;
        public virtual AppUser? User { get; set; } = null!;

        public virtual ICollection<ProjectSkill> ProjectSkills { get; set; }

        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
