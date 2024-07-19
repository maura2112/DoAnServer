using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Favorite
{
    public class FavoriteSearch : SearchDTO
    {
        public int? StatusId { get; set; }

        public int? UserId { get;  set; }
    }
}
