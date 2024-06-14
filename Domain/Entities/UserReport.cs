using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserReport : BaseEntity
    {
        [ForeignKey("AppUser")]
        public int CreatedBy { get; set; }
        public int ReportCategoryId { get; set; }
        public string ReportToUrl { get; set; }


        public bool IsApproved { get; set; }
        public string Description { get; set;}
        public DateTime CreatedDate { get; set;}
        public DateTime? UpdatedDate { get; set;}

        public AppUser User { get; set; } = null!;
        public ReportCategory ReportCategory { get; set; } = null!;



    }
}
