using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AppUserDTO : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
