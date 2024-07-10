using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthenticationDTO
{
    public class LoginDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không đúng định dạng");
            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu nhập vào phải có ít nhất 6 ký tự")
            .MaximumLength(32).WithMessage("Mật khẩu nhập vào phải có nhiều nhất 32 ký tự")
            .Matches(@"^\S*$").WithMessage("Mật khẩu không được chứa dấu cách");
        }
    }

}
