using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class ProjectStatus
    {
        public enum StatusId
        {
            Pending = 1,
            Open = 2,
            Close = 3,
            Reject =5,
            Done = 6,
        }
    }
}
