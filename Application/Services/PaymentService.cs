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
using Domain.Common;
using Application.DTOs;
using AutoMapper;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        public PaymentService(IConfiguration configuration, ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public int MoneyCheckout(int amount)
        {
            int baseAmount = amount * 5000;

            if (amount > 30)
            {
                return (int)(baseAmount * 0.7); // Giảm 30%
            }
            else if (amount > 20)
            {
                return (int)(baseAmount * 0.8); // Giảm 20%
            }
            else if (amount > 10)
            {
                return (int)(baseAmount * 0.9); // Giảm 10%
            }
            else
            {
                return baseAmount; // Không giảm
            }
        }

        public int ReverseMoneyCheckout(int totalAmount)
        {
            int amount = totalAmount / 5000;
            int result;

            if (amount > 30)
            {
                result = (int)(totalAmount / (5000 * 0.7));
            }
            else if (amount > 20)
            {
                result = (int)(totalAmount / (5000 * 0.8)); 
            }
            else if (amount > 10)
            {
                result = (int)(totalAmount / (5000 * 0.9)); 
            }
            else
            {
                result = amount; 
            }
            if (amount > 50)
            {
                result += 10;
            }
            return result;
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

        public int MoneyBuyProjectCheckout(int amount)
        {
            int baseAmount = amount * 20000;

            if (amount > 20)
            {
                return (int)(baseAmount * 0.7); // Giảm 30%
            }
            else if (amount > 10)
            {
                return (int)(baseAmount * 0.8); // Giảm 20%
            }
            else if (amount > 5)
            {
                return (int)(baseAmount * 0.9); // Giảm 10%
            }
            else
            {
                return baseAmount; // Không giảm
            }
        }

        public int ReverseMoneyBuyProjectCheckout(int totalAmount)
        {
            int basePrice = 20000;
            int amount;

            if (totalAmount > 20 * basePrice * 0.7)
            {
                amount = (int)(totalAmount / (basePrice * 0.7)); // Giảm 30%
            }
            else if (totalAmount > 10 * basePrice * 0.8)
            {
                amount = (int)(totalAmount / (basePrice * 0.8)); // Giảm 20%
            }
            else if (totalAmount > 5 * basePrice * 0.9)
            {
                amount = (int)(totalAmount / (basePrice * 0.9)); // Giảm 10%
            }
            else
            {
                amount = totalAmount / basePrice; // Không giảm
            }

            if(amount > 50)
            {
                amount = amount + 10;
            }

            return amount;
        }

        public async Task<Pagination<TransactionDTO>> GetsTransactionsAsync(TransactionSearch search)
        {
            var transactions = _context.Transactions.Include(t => t.User).AsQueryable();
            if(search.FromDate != null) {
                transactions = transactions.Where(x => x.TransactionDateTime > search.FromDate);
            }
            if (search.ToDate != null)
            {
                transactions = transactions.Where(x => x.TransactionDateTime < search.ToDate);
            }
            if(search.UserName != null)
            {
                transactions = transactions.Where(x => x.User.Name.ToLower().Contains(search.UserName.ToLower()));
            }
            if (search.Email != null)
            {
                transactions = transactions.Where(x => x.User.Email.ToLower().Contains(search.Email.ToLower()));
            }

            if (search.counterAccountName != null)
            {
                transactions = transactions.Where(x => x.CounterAccountName.ToLower().Contains(search.counterAccountName.ToLower()));
            }

            if (search.Type != null)
            {
                transactions = transactions.Where(x => x.Type.Equals(search.Type));
            }
            var totalItem = await transactions.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToListAsync();
            var transactionDTOs = _mapper.Map<List<TransactionDTO>>(totalItem);
            var result = new Pagination<TransactionDTO>()
            {
                PageSize = search.PageSize,
                PageIndex = search.PageIndex,
                TotalItemsCount = transactions.Count(),
                Items = transactionDTOs,
            };
            return result;
        }

        public async Task<Pagination<TransactionDTO>> GetsTransactionsByUserIdAsync(SearchDTO search)
        {
            var userId = _currentUserService.UserId;
            var transactions = _context.Transactions.Where(x=>x.UserId== userId).Include(t => t.User).AsQueryable();

            var totalItem = await transactions.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToListAsync();
            var transactionDTOs = _mapper.Map<List<TransactionDTO>>(totalItem);
            var result = new Pagination<TransactionDTO>()
            {
                PageSize = search.PageSize,
                PageIndex = search.PageIndex,
                TotalItemsCount = transactions.Count(),
                Items = transactionDTOs,
            };
            return result;
        }
    }
}
