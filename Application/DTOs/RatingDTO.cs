using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RatingDTO
    {
        public int Id { get; set; } 
        public int? UserId { get; set; }
        public string Comment { get; set; }

        public int Star { get; set; }
        public int? RateTransactionId { get; set; }

        public int? ProjectId { get; set; }

        public string? ProjectName { get; set; }

        public string? UserRate { get; set; }

        public List<string>? SkillOfProject { get; set; }

        public int RateToUserId { get; set; }
    }

    public class RatingDTOValidator : AbstractValidator<RatingDTO>
    {
        public RatingDTOValidator()
        {
            RuleFor(v => v.Star).NotEmpty().GreaterThan(0).LessThan(6).WithMessage("Chỉ được đánh giá từ 1-5 sao");
            RuleFor(v => v.Comment.Length).LessThan(200).WithMessage("Tối đa 200 kí tự");
        }
    }

}
