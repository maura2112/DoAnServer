using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TransactionSearch : SearchDTO
    {
        public DateTime? FromDate {  get; set; }
        public DateTime? ToDate { get; set;}

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Type { get; set; }
    }
}
