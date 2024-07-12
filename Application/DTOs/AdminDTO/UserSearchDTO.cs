using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserSearchDTO: SearchDTO
    {

        public string? role { set; get; }
        public string? search { set; get; }

        public string? email { set; get; }

        public string? phone { set; get; }
        public List<string>? skills { set; get; }
    }
    public class UserSearchDTOValidator : AbstractValidator<UserSearchDTO>
    {
        public UserSearchDTOValidator()
        {
            RuleFor(v => v.role)
                .Must(BeAValidRole).WithMessage("Vai trò không hợp lệ");
            RuleFor(v => v.phone)
    .Matches(@"^\d*$").WithMessage("Số điện thoại chỉ chứa số");
        }
        private bool BeAValidRole(string role)
        {
            return string.IsNullOrEmpty(role) || role == "Freelancer" || role == "Admin" || role == "Recruiter";
        }
    }
}
