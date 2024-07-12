using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BlogCreateDTO
    {
        public int? CreatedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string BlogImage { get; set; }
    }

    public class BlogCreateDTOValidator : AbstractValidator<BlogCreateDTO>
    {
        public BlogCreateDTOValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Tiêu đề không được để trống");
            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Mô tả chi tiết không được để trống");
            RuleFor(v => v.CategoryId)
                .NotEmpty().WithMessage("Danh mục không được để trống");

            RuleFor(v => v.BlogImage)
                .NotEmpty().WithMessage("Ảnh mặc định không được để trống");
        }
    }
}
