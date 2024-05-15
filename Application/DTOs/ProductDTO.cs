
using Domain.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs // chú ý để phải để namespace như này với các DTO, để có thể validate
{
    public class ProductDTO : BaseEntity
    {
        public string? Title { get; set; } = string.Empty;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(v => v.Title).NotEmpty().WithMessage("Bắt buộc");
        }
    }
}
