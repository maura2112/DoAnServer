using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReportSearchDTO : SearchDTO
    {

        public string? typeDes { set; get; }

        public bool? approved { set; get; }
    }
}
