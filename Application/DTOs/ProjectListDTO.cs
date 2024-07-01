using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectListDTO : SearchDTO
    {
        public int UserId { get; set; } 

        public int? StatusId { get; set; }
    }
}
