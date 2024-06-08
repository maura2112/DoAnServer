using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProjectSearchDTO : SearchDTO
    {
        //public int CategoryId { get; set; }
        public string Keyword { get; set; }
    }
    public class ProjectFilter : SearchDTO
    {
        public int CategoryId { get; set; }
        public List<int> SkillIds { get; set; } = new List<int>();
        public int Duration { get; set; }
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }

    }


}
