using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectStatus : BaseEntity
    {
        public string StatusName { get; set; }
        //public string StatusColor { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
