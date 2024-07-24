using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;

namespace API.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly PayOS _payOS;
        public PaymentController(PayOS payOS)
        {
            _payOS = payOS;
        }

        [HttpPost]
        [Route(Common.Url.Payment.Create)]
        public async Task<IActionResult> IndexAsync([FromBody] int amount)
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData("Lần dự thầu", amount, amount*5000);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(orderCode, amount * 5000, "Mua thêm số lượng dự thầu", items, "https://localhost:3002/cancel", "https://localhost:3002/success");
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
        public async Task<IActionResult> Sucess([FromQuery] int amount)
        {
            try
            {
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData("Lần dự thầu", amount, amount * 5000);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(orderCode, amount * 5000, "Mua thêm số lượng dự thầu", items, "https://localhost:3002/cancel", "https://localhost:3002/success");
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Ok(createPayment.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest("Không tạo đơn thành công");
            }
        }
    }
}
