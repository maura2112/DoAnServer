using Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        public int MoneyCheckout(int amount)
        {
            int baseAmount = amount * 5000;

            if (amount > 40)
            {
                return (int)(baseAmount * 0.8);
            }
            else if (amount > 20)
            {
                return (int)(baseAmount * 0.9);
            }
            else
            {
                return baseAmount;
            }
        }
    }
}
