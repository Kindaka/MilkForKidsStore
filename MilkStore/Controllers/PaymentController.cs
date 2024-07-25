﻿using Microsoft.AspNetCore.Http;
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
                    var res = await _paymentService.CancelTransaction(parameters);
                    if (res != null) { 
                        return BadRequest(res);
                    } else
                    {
                        return NotFound("Order does not created");
                    }
                }
                var result = await _paymentService.CreatePayment(parameters);

                if (result != null)
                {
                    string urlSuccess = "https://localhost:7190/PaymentPage/Success";
                    return Redirect(urlSuccess);
                }
                else
                {
                    string urlError = "https://localhost:7190/PaymentPage/Error";
                    return Redirect(urlError);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
