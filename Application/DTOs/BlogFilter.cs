using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BlogFilter
    {
        public int Top {  get; set; }
        public bool? IsHomePage { get; set; }

        public bool? IsHot { get; set; }
    }
}
