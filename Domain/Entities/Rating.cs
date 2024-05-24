using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int UserId {  get; set; }
        public int UserRatingToId { get; set;}

        public string Comment {  get; set; }
        public int Star {  get; set; }

        public virtual AppUser? User { get; set; } = null!;
        public virtual AppUser? UserRatingTo { get; set; } = null!;

    }
}
