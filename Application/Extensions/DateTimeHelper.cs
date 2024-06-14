using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class DateTimeHelper
    {
        public static string ToVietnameseDateString(DateTime? dateTime)
        {

            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                return string.Empty; 
            }
        }
    }
}
