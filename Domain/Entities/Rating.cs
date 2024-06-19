using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int UserId { get; set; }

        public string Comment { get; set; }

        public int Star { get; set; }

        public int RateToUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }


        public bool? IsDelete { get; set; }

        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
