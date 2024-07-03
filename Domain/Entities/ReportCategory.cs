using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReportCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string? ReportCode { get; set; }
        public virtual ICollection<UserReport> UserReports { get; set; }
    }
}
