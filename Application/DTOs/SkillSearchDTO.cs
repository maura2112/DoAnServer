using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SkillSearchDTO : SearchDTO
    {
        public string? SkillName { get; set; }
        public int? CategoryId { get; set;}

    }
}
