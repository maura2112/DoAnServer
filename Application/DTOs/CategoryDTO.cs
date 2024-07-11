using Application.DTOs.AuthenticationDTO;
using Domain.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CategoryDTO : BaseEntity
    {
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }
    public class UpdateCategoryDTO : BaseEntity
    {
        public string CategoryName { get; set; }
        public string Image { get; set; }
    }

    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(v => v.CategoryName).NotEmpty().WithMessage("Tên danh mục không được để trống");
            RuleFor(v => v.CategoryName.Length).LessThan(50).WithMessage("Danh mục tối đa 50 kí tự");
        }
    }
    public class UpdateCategoryDTOValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryDTOValidator()
        {
            RuleFor(v => v.CategoryName).NotEmpty().WithMessage("Tên danh mục không được để trống");
            RuleFor(v => v.CategoryName.Length).LessThan(50).WithMessage("Danh mục tối đa 50 kí tự");
        }
    }
}
