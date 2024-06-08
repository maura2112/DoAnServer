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
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual AddressDTO? Address { get; set; } = null!;
        public string? TaxCode { get; set; }

        public bool IsCompany { get; set; }

        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? Qualifications { get; set; }
        public string? Avatar { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }

}
