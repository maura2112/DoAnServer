using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AppUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual AddressDTO? Address { get; set; } = null!;
        public string? TaxCode { get; set; }

        public string? Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string Description { get; set; }

        public string LockoutEnabled { get; set; }

        public bool IsCompany { get; set; }

        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Qualifications { get; set; }
        public string? Avatar { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public float? AvgRate { get; set; }
        public int? TotalRate { get; set; }
        public int? TotalCompleteProject { get; set; }
    }
    public class AppUserDTO2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool? EmailConfirmed { get; set; }

        public float? AvgRate { get; set; }
        public int? TotalRate { get; set; }
        public int? TotalCompleteProject { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public List<string> Skill { get; set; } = new();
        //public string? CompletedProjectRate { get; set; }
    }

}
