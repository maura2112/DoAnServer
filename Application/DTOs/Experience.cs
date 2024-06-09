using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class Experience 
    {
        public string Title { get; set; }
        public string Company { get; set; }

        public Start Start { get; set; }
        public End End { get; set; }

        public string Summary { get; set; } 

    }
}
