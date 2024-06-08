using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category 

    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
