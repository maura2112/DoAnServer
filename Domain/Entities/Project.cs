using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        [ForeignKey("AppUser")]
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int StatusId { get; set; }

        [ForeignKey("MediaFile")]
        public long? MediaFileId { get; set; }

        public virtual AppUser? AppUser { get; set; } = null!;
        public virtual Category? Category { get; set; } = null!;
        public virtual MediaFile? MediaFile { get; set; } = null!;
        public virtual ProjectStatus? ProjectStatus { get; set; } = null!;
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }

        public virtual ICollection<ProjectSkill> ProjectSkills { get; set; }

    }
}
