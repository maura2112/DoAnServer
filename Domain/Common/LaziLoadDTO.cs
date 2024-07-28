using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class LaziLoadDTO<T>
    {
        public string nextCursor { get; set; }
        public List<T>? Items { get; set; }
    }
}
