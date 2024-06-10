using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SearchDTO
    {
        public int  PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class SearchDTOValidator : AbstractValidator<SearchDTO>
    {
        public SearchDTOValidator()
        {
            RuleFor(v => v.PageIndex).GreaterThan(1).WithMessage("Số trang phải lớn hơn 1");
            RuleFor(v => v.PageSize).GreaterThan(1).WithMessage("Kích cỡ trang phải lớn hơn 1");
        }
    }
}
