using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReportCreateDTO
    {
        public string? ReportToUrl { get; set; }
        public int? ProjectId { get; set; }

        public int? BidId { get; set; }
        public int? CreatedBy { get; set; }
        public string Description { get; set; }
        public int ReportCategoryId { get; set; }
    }
}
