using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using MimeKit.Encodings;

namespace Application.DTOs
{
    public class ProjectStatusDTO :BaseEntity
    {
        public string StatusName { get; set; }
        public string? StatusColor { get; set; }
    }
}
