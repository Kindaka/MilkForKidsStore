using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.CartDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartItemService;
        private readonly string _imagesDirectory;

        public CartController(ICartService cartItemService, IWebHostEnvironment env)
        {
            _cartItemService = cartItemService;
            _imagesDirectory = Path.Combine(env.ContentRootPath, "img", "product");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartDtoRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Cannot add empty object to cart");
                }
                var status = await _cartItemService.AddToCart(request);
                if (status)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Cannot add this project to cart because of larger quantity than in stock");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{CustomerId}")]
        public async Task<IActionResult> GetCustomerCart(int CustomerId)
        {
            try
            {
                var response = await _cartItemService.GetCartByCustomerId(CustomerId);
                if (!response.Any())
                {
                    return NotFound("No item in your cart");
                }
                foreach (var item in response)
                {
                    if (item.ProductView.Images.Any())
                    {
                        foreach (var image in item.ProductView.Images)
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemInCart(int id)
        {
            try
            {
                var check = await _cartItemService.DeleteItemInCart(id);
                if (check)
                {
                    return Ok("Delete successfully");
                }
                else
                {
                    return BadRequest("Item does not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("api/v1/[controller]/Quantity")]
        public async Task<IActionResult> UpdateItemQuantityInCart([FromQuery] int CartId, [FromQuery] int Quantity)
        {
            try
            {
                var response = await _cartItemService.UpdateItemQuantityInCart(CartId, Quantity);
                if (response == 1)
                {
                    return Ok("Update quantity success");
                }
                else if (response == 3)
                {
                    return Ok("Remove item success");
                }
                else if (response == 2)
                {
                    return BadRequest("Your quantity is greater than number of product in stock");
                }
                else if (response == -1)
                {
                    return BadRequest("Product is not exist");
                }
                else if (response == 0)
                {
                    return BadRequest("Item in cart is not exist");
                }
                return BadRequest("Cannot update");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
