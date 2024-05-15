using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs // chú ý để phải để namespace như này với các DTO, để có thể validate
{
    public class ProductSearchDTO : SearchDTO
    {
        public string? Title { get; set; }
    }

    public class ProductSearchValidator : AbstractValidator<ProductSearchDTO>
    {
        public ProductSearchValidator()
        {
            RuleFor(v => v.Title).NotEqual("Bánh").WithMessage("Không bằng bánh");
            RuleFor(v => v.PageIndex).GreaterThan(0).WithMessage("Phải lớn hơn 0");
        }
    }
}
