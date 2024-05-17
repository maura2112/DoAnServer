using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UrlRecord 
    {
        public long Id { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
    }
}
