using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class Education
    {
        public string Country { get; set; }
        public string UniversityCollege { get; set; }
        public string Degree { get; set; }
        public Start Start { get; set; }
        public End End { get; set; }
    }
}
