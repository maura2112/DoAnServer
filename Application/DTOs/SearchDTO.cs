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
        //public SearchDTOValidator()
        //{
        //    RuleFor(v => v.PageIndex).GreaterThan(0).WithMessage("Số trang phải lớn hơn 0");
        //    RuleFor(v => v.PageSize).InclusiveBetween(1, 10).WithMessage("Kích cỡ trang trong khoảng 1-10");
        //}
    }
}
