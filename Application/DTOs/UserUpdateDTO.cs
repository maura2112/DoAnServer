using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserUpdateDTO
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public string? Avatar { set; get; }
        public string Description { set; get; }
        public string? TaxCode { set; get; }
        public bool? IsCompany { set; get; }
        public List<string> Skills { set; get; }
    }

    public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateDTOValidator()
        {
            RuleFor(v => v.Name)
    .NotEmpty().WithMessage("Bắt buộc")
    .Matches("^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểễệỉịọỏốồổỗộớờởỡợỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừửữựỲỴÝỶỸửữựỳỵỷỹ\\s]+$").WithMessage("Tên không được chứa số hoặc ký tự đặc biệt");

            RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email là bắt buộc")
            .EmailAddress().WithMessage("Định dạng email không hợp lệ.");
        }
    }
}
