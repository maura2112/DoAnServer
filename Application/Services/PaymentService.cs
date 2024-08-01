using Application.IServices;
using Net.payOS.Errors;
using Net.payOS.Types;
using Net.payOS.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public PaymentService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
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
        public int ReverseMoneyCheckout(int totalAmount)
        {
            int amount = totalAmount / 5000;

            if (amount > 40)
            {
                return (int)(totalAmount / (5000 * 0.8));
            }
            else if (amount > 20)
            {
                return (int)(totalAmount / (5000 * 0.9));
            }
            else
            {
                return amount;
            }
        }

        public async Task<PaymentLinkInformation> getPaymentLinkInfomation(int orderId)
        {
            var payOSClientId = _configuration["Environment:PAYOS_CLIENT_ID"];
            var payOSApiKey = _configuration["Environment:PAYOS_API_KEY"];
            var payOSChecksumKey = _configuration["Environment:PAYOS_CHECKSUM_KEY"];
            string url = "https://api-merchant.payos.vn/v2/payment-requests/" + orderId;
            HttpClient httpClient = new HttpClient();
            JObject responseBodyJson = JObject.Parse(await (await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers =
            {
                { "x-client-id", payOSClientId },
                { "x-api-key", payOSApiKey }
            }
            })).Content.ReadAsStringAsync());
            string code = responseBodyJson["code"]?.ToString();
            string desc = responseBodyJson["desc"]?.ToString();
            string data = responseBodyJson["data"]?.ToString();
            if (code == null)
            {
                throw new PayOSError("20", "Internal Server Error.");
            }

            if (code == "00" && data != null)
            {
                PaymentLinkInformation response = JsonConvert.DeserializeObject<PaymentLinkInformation>(data);
                return response;
            }
            return null;
        }

        public async Task<Domain.Entities.Transaction> GetByOrderId(string orderId)
        {
            var trans = await _context.Transactions.FirstOrDefaultAsync(x=>x.OrderCode.Equals(orderId));
            return trans;
        }

        public async Task<bool> AddTransaction(Domain.Entities.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
