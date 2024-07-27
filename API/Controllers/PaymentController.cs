using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace API.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        public PaymentController(PayOS payOS, IPaymentService paymentService, ICurrentUserService currentUserService, UserManager<AppUser> userManager)
        {
            _payOS = payOS;
            _paymentService = paymentService;
            _currentUserService = currentUserService;
            _userManager = userManager;
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
                PaymentData paymentData = new PaymentData(orderCode, total, "Mua thêm số lượng dự thầu", items, "https://localhost:3002/cancel", "https://localhost:3002/success");
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
        [Route(Common.Url.Payment.Sucess)]
        public async Task<IActionResult> Sucess([FromQuery] PaymentNotification notification)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return BadRequest("Không tìm thấy tài khoản phù hợp");
                }
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInfomation(int.Parse(notification.OrderCode));
                var totalBids = _paymentService.ReverseMoneyCheckout(paymentLinkInformation.amount);
                user.AmountBid = user.AmountBid + totalBids;
                await _userManager.UpdateAsync(user);
                return Ok(user.AmountBid);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest("Không tạo đơn thành công");
            }
        }
    }
}
