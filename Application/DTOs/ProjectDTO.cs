using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectDTO : BaseEntity
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }
        public string Duration { get; set; }
        public int? CreatedBy { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int StatusId { get; set; }
        public long? MediaFileId { get; set; }
        
        List<ProjectStatus> ListStatus { get; set; }

        public virtual AppUserDTO? AppUser { get; set; } = null!;
        public virtual CategoryDTO? Category { get; set; } = null!;
        //public virtual Category? Category { get; set; } = null!;
        //public virtual MediaFile? MediaFile { get; set; } = null!;
        //public virtual ProjectStatus? ProjectStatus { get; set; } = null!;
    }
}
