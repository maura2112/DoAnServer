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
        public async Task<IActionResult> IndexAsync([FromBody] int amount)
        {
            try
            {
                var total = _paymentService.MoneyCheckout(amount);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData("Lần dự thầu", amount, total);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                var userId = _currentUserService.UserId;
                PaymentData paymentData = new PaymentData(orderCode, total, "Mua thêm số lượng dự thầu", items, "http://localhost:5069/cancel", "https://webapp-doan-2.azurewebsites.net/api/Payment/Success?userId=" + userId);
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
        public async Task<IActionResult> Sucess([FromQuery] int userId,[FromQuery] string orderCode)
        {
            try
            {
                
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return BadRequest("Không tìm thấy tài khoản phù hợp");
                }
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInfomation(int.Parse(orderCode));
                var totalBids = _paymentService.ReverseMoneyCheckout(paymentLinkInformation.amount);
                user.AmountBid = user.AmountBid + totalBids;
                await _userManager.UpdateAsync(user);

                NotificationDto notificationDto = new NotificationDto()
                {
                    NotificationId = await _notificationRepository.GetNotificationMax() + 1,
                    SendId = userId,
                    SendUserName = "",
                    ProjectName = "",//k can cx dc
                    RecieveId = userId,
                    Description = "Bạn đã nạp thành công "+totalBids +" lần đấu thầu",
                    Datetime = DateTime.Now,
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
    }
}
