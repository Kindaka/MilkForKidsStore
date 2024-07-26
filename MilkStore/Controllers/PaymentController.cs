using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.PaymentDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private static readonly string URL_SUCCESS = "https://localhost:7190/PaymentPage/Success";
        private static readonly string URL_ERROR = "https://localhost:7190/PaymentPage/Error";

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> CreatePayment([FromQuery] PaymentDtoRequest parameters)
        {
            try
            {
                if (parameters.vnp_BankTranNo == null)
                {
                    var res = await _paymentService.CancelTransaction(parameters);
                    if (res != null) {
                        return Redirect(URL_ERROR);
                    } else
                    {
                        return NotFound("Order does not created");
                    }
                }
                var result = await _paymentService.CreatePayment(parameters);

                if (result != null)
                {
                    return Redirect(URL_SUCCESS);
                }
                else
                {
                    return Redirect(URL_ERROR);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
