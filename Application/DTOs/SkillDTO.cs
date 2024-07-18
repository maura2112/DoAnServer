using Domain.Common;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SkillDTO : BaseEntity
    {
        public int? Id { get; set; }
        public int CategoryId { get; set; }
        public string SkillName { get; set; }

        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedTime { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedTime { get; set; }

        public string? CategoryName { get; set; }
        //public virtual ICollection<ProjectSkillDTO> ProjectSkills { get; set; }

        //public virtual ICollection<UserSkillDTO> UserSkills { get; set; }
    }
    public class SkillDTOValidator : AbstractValidator<SkillDTO>
    {
        public SkillDTOValidator()
        {
            RuleFor(v => v.SkillName)
                .NotEmpty().WithMessage("Kĩ năng không được để trống")
                .MaximumLength(200).WithMessage("Kĩ năng không vượt quá 200 kí tự");
            RuleFor(v => v.CategoryId)
                .NotEmpty().WithMessage("Loại kĩ năng không được để trống");
        }
    }
    public class SkillListByCate 
    {
        public int CategoryId { get; set; }
    }
}
