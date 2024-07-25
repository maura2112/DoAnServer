using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Application.DTOs;
using Application.IServices;

namespace API.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        public PaymentController(PayOS payOS, IPaymentService paymentService)
        {
            _payOS = payOS;
            _paymentService = paymentService;
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
                return Ok(createPayment.checkoutUrl);
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
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInfomation(int.Parse(notification.OrderCode));

                return Ok();
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest("Không tạo đơn thành công");
            }
        }
    }
}
