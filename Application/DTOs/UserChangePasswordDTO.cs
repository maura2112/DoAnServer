using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserChangePasswordDTO
    {
        public string OldPassword { set; get; }
        public string NewPassword { set; get; }

        public string NewPasswordConfirm { set; get; }
    }
    public class UserChangePasswordDTOValidator : AbstractValidator<UserChangePasswordDTO>
    {
        public UserChangePasswordDTOValidator()
        {
            RuleFor(v => v.OldPassword).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.NewPassword)
            .Equal(v => v.NewPasswordConfirm)
            .WithMessage("Xác nhận mật khẩu không khớp");
        }
    }
}
