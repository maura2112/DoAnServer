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
            RuleFor(v => v.Email).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.Password).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.Roles).NotEmpty().WithMessage("Bắt buộc");
            RuleFor(v => v.Password)
            .Equal(v => v.ConfirmPassword)
            .WithMessage("Xác nhận mật khẩu không khớp");
        }
    }
}
