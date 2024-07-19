using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Favorite
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public int ProjectId { get; set; }

        public string CreatedProject { get; set; }

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }

        public string SavedTime { get; set; }

        public string CreatedProjectTime { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
        public string StatusColor { get; set; }

        public int StatusId { get; set; }   
    }
}
