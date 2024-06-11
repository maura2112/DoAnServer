using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Qualification>? Qualifications { get; set; }
        public List<Experience>? Experiences { get; set; }
        public List<Education>? Educations { get; set; }
        public string? Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompany { get; set; }
        public string? Description { get; set; }

        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? EmailConfirmed { get; set; }
        public List<MediaFileDTO> mediaFiles { get; set; }

        public List<string>? skills { get; set; }

        public AddressDTO Address { get; set; }

    }
}
