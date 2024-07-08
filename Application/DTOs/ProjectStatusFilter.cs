using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectStatusFilter : SearchDTO
    {
        public int? statusId {  get; set; }
        public int? userId { get; set; }
    }
}
