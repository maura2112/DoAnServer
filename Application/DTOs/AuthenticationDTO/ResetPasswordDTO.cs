using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
        public string SecureToken { get; set; }
    }

    public class ResetPasswordCodeDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }

    }
    public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordDTOValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.NewPassword).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.SecureToken).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.NewPassword)
            .Equal(v => v.NewPasswordConfirm)
            .WithMessage("Xác nhận mật khẩu không khớp");
        }
    }
}
