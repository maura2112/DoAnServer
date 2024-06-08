using Domain.Common;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectDTO : BaseEntity
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }
        public int Duration { get; set; }
        public int? CreatedBy { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int StatusId { get; set; }
        public long? MediaFileId { get; set; }
        
        List<ProjectStatus> ListStatus { get; set; }

        public virtual AppUserDTO? AppUser { get; set; } = null!;
        public virtual CategoryDTO? Category { get; set; } = null!;

        public List<string> Skill { get; set; } = new();
        //public List<SkillDTO> SkillList { get; set; } 

        //public virtual SkillDTO? Skill { get; set; } = null!;
        //public virtual MediaFile? MediaFile { get; set; } = null!;
        public virtual ProjectStatusDTO? ProjectStatus { get; set; } = null!;
    }

    public class ProjectDTOValidator : AbstractValidator<ProjectDTO>
    {
        public ProjectDTOValidator()
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Title không được để trống");
            RuleFor(v => v.CategoryId).NotEmpty().WithMessage("Phải chọn 1 danh mục");
            RuleFor(v => v.MinBudget).NotEmpty().WithMessage("Không được để trống");
            RuleFor(v => v.MaxBudget).NotEmpty().WithMessage("Không được để trống");
            RuleFor(v => v.Duration).NotEmpty().WithMessage("Không được để trống");
            RuleFor(v => v.Skill).NotEmpty().WithMessage("Không được để trống");
            RuleFor(v => v.Description).NotEmpty().WithMessage("Không được để trống");
            RuleFor(v => v.MaxBudget).GreaterThan(v => v.MinBudget).WithMessage("Ngân sách tối đa phải lớn hơn ngân sách tối thiểu");
            
        }
    }
}
