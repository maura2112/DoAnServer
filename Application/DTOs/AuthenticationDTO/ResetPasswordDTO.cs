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
    public class EmailConfirmDTO
    {
        public string SecureToken { get; set; }
    }

    public class InputCodeConfirmDTO
    {
        public string Code { get; set; }
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
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email phải đúng định dạng");

            RuleFor(v => v.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu có ít nhất 6 kí tự")
                .MaximumLength(32).WithMessage("Mật khẩu có tối đa 32 kí tự");

            RuleFor(v => v.SecureToken).NotEmpty().WithMessage("SecureToken không được để trống");
            RuleFor(v => v.NewPassword)
            .Equal(v => v.NewPasswordConfirm)
            .NotEmpty().WithMessage("Mật khẩu nhập lại không được để trống")
            .WithMessage("Xác nhận mật khẩu không khớp");
        }
    }
    public class ResetPasswordCodeDTOValidator : AbstractValidator<ResetPasswordCodeDTO>
    {
        public ResetPasswordCodeDTOValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email phải đúng định dạng");
            RuleFor(v => v.Code)
                .NotEmpty().WithMessage("Mã xác nhận không được để trống");
        }
    }
}
