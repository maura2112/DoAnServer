using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Favorite
{
    public class FavoriteCreate
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int ProjectId { get; set; }
    }
}
