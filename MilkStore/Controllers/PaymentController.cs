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
                    return BadRequest("Transaction failed");
                }
                var result = await _paymentService.CreatePayment(parameters);

                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Order does not created");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
