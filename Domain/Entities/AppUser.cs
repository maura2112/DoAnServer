using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<int>    
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
        public virtual Address? Address { get; set; } = null!;
        public virtual Rating? Rating { get; set; } = null!;

        public virtual ICollection<AppUser> UserRatingTo { get; set; }

        public virtual ICollection<Portfolio> Portfolios { get; set; }
    }
}
