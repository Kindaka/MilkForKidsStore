using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.CustomerDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(customerId);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomerInfo(int customerId, [FromBody] UpdateCustomerDto updateDto)
        {
            try
            {
                var result = await _customerService.UpdateCustomerInfoAsync(customerId, updateDto);
                if (!result)
                {
                    return StatusCode(500, "Error updating customer info");
                }
                return Ok(new { success = true, message = "Customer info updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal Server Error: {ex.Message}" });
            }
        }
    }
}
