using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.VoucherOfShopDTOs;
using MilkStore_BAL.Services.Interfaces;

namespace MilkStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherOfShopController : ControllerBase
    {
        private readonly IVoucherOfShopService _voucherOfShopService;
        public VoucherOfShopController(IVoucherOfShopService voucherOfShopService)
        {
            _voucherOfShopService = voucherOfShopService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _voucherOfShopService.Get();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("{voucherId}")]
        public async Task<IActionResult> Get(int voucherId)
        {
            try
            {
                var response = await _voucherOfShopService.Get(voucherId);
                if (response == null)
                {
                    return NotFound($"Cannot find voucher match id {voucherId}");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetByAdmin()
        {
            try
            {
                var response = await _voucherOfShopService.GetByAdmin();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpGet("admin/{voucherId}")]
        public async Task<IActionResult> GetByAdmin(int voucherId)
        {
            try
            {
                var response = await _voucherOfShopService.GetByAdmin(voucherId);
                if (response == null)
                {
                    return NotFound($"Cannot find voucher match id {voucherId}");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpPost]
        public async Task<IActionResult> Post (VoucherOfShopDtoRequest voucherOfShopDTORequest)
        {
            try
            {
                if(voucherOfShopDTORequest == null)
                {
                    return BadRequest("VoucherOfShop is null");
                }
                if(voucherOfShopDTORequest.StartDate > voucherOfShopDTORequest.EndDate)
                {
                    return BadRequest("EndDate cannot be sooner than StartDate");
                }
                await _voucherOfShopService.Post(voucherOfShopDTORequest);
                return Ok("Create success");
            }
            catch (Exception ex) { 
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpPut("{voucherId}")]
        public async Task<IActionResult> Put(int voucherId, [FromBody] VoucherOfShopDtoRequest voucherOfShopDTORequest)
        {
            try
            {
                if (voucherOfShopDTORequest == null)
                {
                    return BadRequest("VoucherOfShop is null");
                }
                if (voucherOfShopDTORequest.StartDate > voucherOfShopDTORequest.EndDate)
                {
                    return BadRequest("EndDate cannot be sooner than StartDate");
                }
                var response = await _voucherOfShopService.Put(voucherId, voucherOfShopDTORequest);
                if (response)
                {
                    return Ok("Update success");
                } else
                {
                    return NotFound($"Cannot find voucher match id {voucherId}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpPut("voucherId={voucherId}&status={status}")]
        public async Task<IActionResult> UpdateStatus(int voucherId, bool status)
        {
            try
            {
                var response = await _voucherOfShopService.UpdateStatus(voucherId, status);
                if (response)
                {
                    return Ok("Update success");
                }
                else
                {
                    return NotFound($"Cannot find voucher match id {voucherId}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
