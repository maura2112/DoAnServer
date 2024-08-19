using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TransactionDTO
    {
        public string Id { get; set; }
        public string OrderCode { get; set; }
        public int Amount { get; set; }

        public string Type { get; set; }
        public int TotalMoney { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string Description { get; set; }
        public string counterAccountBankId { get; set; }
        public string CounterAccountName { get; set; }
        public string CounterAccountNumber { get; set; }
        public string reference { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class TransactionTracking
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalMoney { get; set; }
    }
}
