using Net.payOS.Types;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Common;

namespace Application.IServices
{
    public interface IPaymentService
    {
        public int MoneyCheckout(int amount);
        public int ReverseMoneyCheckout(int totalAmount);


        public int MoneyBuyProjectCheckout(int amount);

        public int ReverseMoneyBuyProjectCheckout(int totalAmount);

        public Task<Domain.Entities.Transaction> GetByOrderId(string orderId);
        public Task<PaymentLinkInformation> getPaymentLinkInfomation(int orderId);
        public Task<bool> AddTransaction (Domain.Entities.Transaction transaction);

        public Task<Pagination<TransactionDTO>> GetsTransactionsAsync(TransactionSearch search);
        public Task<Pagination<TransactionDTO>> GetsTransactionsByUserIdAsync(SearchDTO search);
    }
}
