using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RatingDTO
    {
        public int? UserId { get; set; }
        public string Comment { get; set; }

        public int Star { get; set; }

        public int RateToUserId { get; set; }
    }
}
