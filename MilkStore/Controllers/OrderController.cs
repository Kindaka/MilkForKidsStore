using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_BAL.Services.Interfaces;
using MilkStore_DAL.Entities;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly string _imagesDirectory;

        public OrderController(IOrderService orderService, IWebHostEnvironment env)
        {
            _orderService = orderService;
            _imagesDirectory = Path.Combine(env.ContentRootPath, "img", "product");
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            try
            {
                var response = await _orderService.Get();
                return Ok(response);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("detail/{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            try
            {
                var response = await _orderService.Get(orderId);
                if (response != null)
                {
                    foreach (var item in response.orderDetails)
                    {
                        if (item.product.Images.Any())
                        {
                            foreach (var image in item.product.Images)
                            {
                                var imagePath = Path.Combine(_imagesDirectory, image.ImageProduct1);
                                if (System.IO.File.Exists(imagePath))
                                {
                                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                                    image.ImageProduct1 = Convert.ToBase64String(imageBytes);
                                }
                            }
                        }
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            try
            {
                var response = await _orderService.GetByCustomerId(customerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("createOrder")]
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
