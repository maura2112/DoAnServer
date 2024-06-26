using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public string? NameCreatedBy { get; set; }
        public string? ReportToUrl { get; set; }
        public int? ProjectId { get; set; }

        public int? BidId { get; set; }
        public int? CreatedBy { get; set; }
        public int ReportCategoryId { get; set; }
        public string Description { get; set; }

        public string? ReportType { get; set; }

        public string? ReportName { get; set; }

        public string? ProjectName { get; set; }

        public string? ProjectUser { get; set; }

        public string? BidName { get; set; }

        public string? BidUser { get; set; }

        public bool? IsApproved { get; set; }
    }
}
