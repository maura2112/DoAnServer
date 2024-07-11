using Domain.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectSearchDTO : SearchDTO
    {
        //public int CategoryId { get; set; }
        public string? Keyword { get; set; }

        public List<string>? Skill { get; set; }

        public int? StatusId { get; set; }

        public int? MinBudget { get; set; }
        public int? MaxBudget { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }


    }
    public class ProjectFilter : SearchDTO
    {
        public string? Keyword { get; set; }
        public int CategoryId { get; set; }
        public List<int> SkillIds { get; set; } = new List<int>();
        public int Duration { get; set; }
        public int MinBudget { get; set; } 
        public int MaxBudget { get; set; } 

    }

    public class ProjectStatus 
    {
        public int Id { get; set; }
        public int StatusId { get; set; }

    }

    public class ProjectSearchDTOValidator : AbstractValidator<ProjectSearchDTO>
    {
        public ProjectSearchDTOValidator()
        {
            RuleFor(v => v.Keyword.Length).LessThan(50).WithMessage("Từ khóa không được vượt quá 50 kí tự");



        }
    }






}
