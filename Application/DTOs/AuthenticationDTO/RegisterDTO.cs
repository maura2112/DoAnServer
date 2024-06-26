using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
        public string Name { get; set; }

        public string? TaxCode { get; set; }

        public bool IsCompany { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Skill { get; set; } = new();
    }
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không đúng định dạng");
            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống");
                //.MinimumLength(6).WithMessage("Mật khẩu nhập vào phải có ít nhất 6 ký tự")
                //.MaximumLength(32).WithMessage("Mật khẩu nhập vào phải có nhiều nhất 32 ký tự");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Tên không được để trống")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Tên không được chứa số hoặc kí tự đặc biệt")
                .MaximumLength(29).WithMessage("Tên không được quá 30 kí tự");

            RuleFor(v => v.Roles).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.Password)
            .Equal(v => v.ConfirmPassword)
            .WithMessage("Xác nhận mật khẩu không khớp");
            RuleFor(v => v.Skill).NotEmpty().WithMessage("Kĩ năng không được để trống");
            RuleFor(v => v.Skill.Count).LessThan(5).WithMessage("Không được chọn quá 5 kĩ năng");
        }
    }
}
