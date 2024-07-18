using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("/createOrder")]
        public async Task<IActionResult> CreateOrder(List<OrderProductDto> cartItems, int? voucherId, int exchangedPoint)
        {
            try
            {
                if (!cartItems.Any())
                {
                    return BadRequest("No item to order");
                }
                var checkItem = await _orderService.ValidateItemInCart(cartItems);
                if (checkItem == -1)
                {
                    return BadRequest("Some items that not valid in your cart");
                }
                else if (checkItem == -2)
                {
                    return BadRequest("Some items that are not available now");
                }
                else if (checkItem == -3)
                {
                    return BadRequest("Some items that have higher quantity than quantity in stock");
                }
                else
                {
                    if (voucherId != null) {
                        var checkVoucher = await _orderService.CheckVoucher((int) voucherId);
                        if (!checkVoucher)
                        {
                            return BadRequest("Cannot use this voucher");
                        }
                    }
                    if(exchangedPoint > 0)
                    {
                        var checkPoint = await _orderService.ValidateExchangedPoint(exchangedPoint, cartItems[0].customerId);
                        if (!checkPoint)
                        {
                            return BadRequest("Not enough point to exchange");
                        }
                    }
                    var url = await _orderService.CreateOrder(cartItems, voucherId, exchangedPoint);
                    return Ok(new { url = url });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
