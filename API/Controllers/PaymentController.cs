using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Net.payOS.Errors;
using Net.payOS.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationService _notificationService;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _chatHubContext;
        public PaymentController(PayOS payOS, IPaymentService paymentService, ICurrentUserService currentUserService, UserManager<AppUser> userManager, INotificationRepository notificationRepository, INotificationService notificationService, ApplicationDbContext context, IHubContext<ChatHub> chatHubContext)
        {
            _payOS = payOS;
            _paymentService = paymentService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _notificationRepository = notificationRepository;
            _notificationService = notificationService;
            _context = context;
            _chatHubContext = chatHubContext;
        }

        [HttpPost]
        [Route(Common.Url.Payment.Create)]
        public async Task<IActionResult> Create([FromBody] int amount)
        {
            try
            {
                var total = _paymentService.MoneyCheckout(amount);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData("Lần dự thầu", amount, total);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                var userId = _currentUserService.UserId;
                PaymentData paymentData = new PaymentData(orderCode, total, $"DuThau-{userId}-{total}", items, "https://webapp-doan-2.azurewebsites.net/api/Payment/Cancel", "https://webapp-doan-2.azurewebsites.net/api/Payment/Success?userId=" + userId);
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Ok(createPayment);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest("Không tạo đơn thành công");
            }
        }
        [HttpGet]
        [Route(Common.Url.Payment.Success)]
        public async Task<IActionResult> Sucess([FromQuery] int userId, [FromQuery] string orderCode)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return BadRequest("Không tìm thấy tài khoản phù hợp");
                }
                var transaction = await _paymentService.GetByOrderId(orderCode);
                if (transaction != null)
                {
                    return BadRequest("Giao dịch thất bại hoặc lỗi hệ thống");
                }
                PaymentLinkInformation paymentLinkInformation = await _paymentService.getPaymentLinkInfomation(int.Parse(orderCode));
                if (paymentLinkInformation == null)
                {
                    return BadRequest("Không tìm thấy khoản thanh toán");
                }
                var totalBids = _paymentService.ReverseMoneyCheckout(paymentLinkInformation.amount);
                user.AmountBid = user.AmountBid + totalBids;
                await _userManager.UpdateAsync(user);
                var first = paymentLinkInformation.transactions.First();
                var transactionNew = new Domain.Entities.Transaction()
                {
                    Id = paymentLinkInformation.id,
                    OrderCode = paymentLinkInformation.orderCode.ToString(),
                    Amount = totalBids,
                    TotalMoney = paymentLinkInformation.amountPaid,
                    Type = "Dự thầu",
                    CreateAt = DateTime.Parse(paymentLinkInformation.createdAt),
                    TransactionDateTime = DateTime.Parse(first.transactionDateTime),
                    Description = first.description,
                    counterAccountBankId = first.counterAccountBankId,
                    CounterAccountName = first.counterAccountName,
                    CounterAccountNumber = first.counterAccountNumber,
                    reference = first.reference,
                    UserId = userId
                };
                var result = await _paymentService.AddTransaction(transactionNew);
                NotificationDto notificationDto = new NotificationDto()
                {
                    NotificationId = await _notificationRepository.GetNotificationMax() + 1,
                    SendId = userId,
                    SendUserName = "",
                    ProjectName = "",//k can cx dc
                    RecieveId = userId,
                    Description = "Bạn đã nạp thành công " + totalBids + " lần đấu thầu",
                    Datetime = DateTime.UtcNow,
                    NotificationType = 1,
                    IsRead = 0,
                    Link = "#"
                };
                bool x = await _notificationService.AddNotification(notificationDto);
                if (x)
                {
                    var hubConnections = await _context.HubConnections
                                .Where(con => con.userId == userId).ToListAsync();
                    foreach (var hubConnection in hubConnections)
                    {
                        await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", notificationDto);
                    }
                }
                return Ok(user.AmountBid);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route(Common.Url.Payment.Cancel)]
        public async Task<IActionResult> Cancel()
        {
            return BadRequest("Giao dịch không thành công");
        }


        [HttpPost]
        [Route(Common.Url.Payment.CreateBuyProject)]
        public async Task<IActionResult> CreateBuyProject([FromBody] int amount)
        {
            try
            {
                var total = _paymentService.MoneyBuyProjectCheckout(amount);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData("Lần đăng bài dự án", amount, total);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                var userId = _currentUserService.UserId;
                PaymentData paymentData = new PaymentData(orderCode, total, $"DuAn-{userId}-{total}", items, "https://webapp-doan-2.azurewebsites.net/api/Payment/CancelBuyProject", "https://webapp-doan-2.azurewebsites.net/api/Payment/SuccessBuyProject?userId=" + userId);
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Ok(createPayment);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest("Không tạo đơn thành công");
            }
        }


        [HttpGet]
        [Route(Common.Url.Payment.SuccessBuyProject)]
        public async Task<IActionResult> SuccessBuyProject([FromQuery] int userId, [FromQuery] string orderCode)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return BadRequest("Không tìm thấy tài khoản phù hợp");
                }
                var transaction = await _paymentService.GetByOrderId(orderCode);
                if (transaction != null)
                {
                    return BadRequest("Giao dịch thất bại hoặc lỗi hệ thống");
                }
                PaymentLinkInformation paymentLinkInformation = await _paymentService.getPaymentLinkInfomation(int.Parse(orderCode));
                if (paymentLinkInformation == null)
                {
                    return BadRequest("Không tìm thấy khoản thanh toán");
                }
                var totalPrjects = _paymentService.ReverseMoneyCheckout(paymentLinkInformation.amount);
                user.AmoutProject = user.AmoutProject + totalPrjects;
                await _userManager.UpdateAsync(user);
                var first = paymentLinkInformation.transactions.First();
                var transactionNew = new Domain.Entities.Transaction()
                {
                    Id = paymentLinkInformation.id,
                    OrderCode = paymentLinkInformation.orderCode.ToString(),
                    Amount = totalPrjects,
                    TotalMoney = paymentLinkInformation.amountPaid,
                    Type = "Dự án",
                    CreateAt = DateTime.Parse(paymentLinkInformation.createdAt),
                    TransactionDateTime = DateTime.Parse(first.transactionDateTime),
                    Description = first.description,
                    counterAccountBankId = first.counterAccountBankId,
                    CounterAccountName = first.counterAccountName,
                    CounterAccountNumber = first.counterAccountNumber,
                    reference = first.reference,
                    UserId = userId
                };
                var result = await _paymentService.AddTransaction(transactionNew);
                NotificationDto notificationDto = new NotificationDto()
                {
                    NotificationId = await _notificationRepository.GetNotificationMax() + 1,
                    SendId = userId,
                    SendUserName = "",
                    ProjectName = "",//k can cx dc
                    RecieveId = userId,
                    Description = "Bạn đã nạp thành công " + totalPrjects + " lần đăng dự án",
                    Datetime = DateTime.UtcNow,
                    NotificationType = 1,
                    IsRead = 0,
                    Link = "#"
                };
                bool x = await _notificationService.AddNotification(notificationDto);
                if (x)
                {
                    var hubConnections = await _context.HubConnections
                                .Where(con => con.userId == userId).ToListAsync();
                    foreach (var hubConnection in hubConnections)
                    {
                        await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedNotification", notificationDto);
                    }
                }
                return Ok(user.AmoutProject);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest(exception);
            }
        }


        [HttpGet]
        [Route(Common.Url.Payment.CancelBuyProject)]
        public async Task<IActionResult> CancelBuyProject()
        {
            return BadRequest("Giao dịch không thành công");
        }




    }
}
