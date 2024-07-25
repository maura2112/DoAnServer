using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PaymentNotification
    {
        public string Bin { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public string Currency { get; set; }
        public string PaymentLinkId { get; set; }
        public string Status { get; set; }
        public string CheckoutUrl { get; set; }
        public string QrCode { get; set; }
    }
}
