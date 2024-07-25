using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.ChatDTOs;
using MilkStore_BAL.Services.Implements;
using MilkStore_BAL.Services.Interfaces;


namespace MilkStore.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IAuthorizeService _authorizeService;

        public MessageController(IMessageService messageService, IAuthorizeService authorizeService)
        {
            _messageService = messageService;
            _authorizeService = authorizeService;
        }

        [Authorize(Policy = "RequireCustomerRole")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDtoRequest request)
        {
            try
            {
                var accountId = User.FindFirst("AccountId")?.Value;
                if (accountId == null)
                {
                    return Forbid();
                }
                var checkMatchedId = await _authorizeService.CheckAuthorizeByCustomerId(request.CustomerId, int.Parse(accountId));
                if (!checkMatchedId.isMatchedCustomer)
                {
                    return Forbid();
                }
                await _messageService.SendMessage(request);
                return Ok(new { success = true, message = "Message sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpPost("staff")]
        public async Task<IActionResult> SendMessageAdmin([FromBody] MessageDtoRequest request)
        {
            try
            {
                await _messageService.SendMessageAdmin(request);
                return Ok(new { success = true, message = "Message sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Internal Server Error: {ex.Message}" });
            }
        }

        [Authorize(Policy = "RequireStaffOrCustomerRole")]
        [HttpGet("history/{CustomerId}")]
        public async Task<IActionResult> GetChatHistoryByCustomerId(int CustomerId)
        {
            try
            {
                var accountId = User.FindFirst("AccountId")?.Value;
                if (accountId == null)
                {
                    return Forbid();
                }
                var checkMatchedId = await _authorizeService.CheckAuthorizeByCustomerId(CustomerId, int.Parse(accountId));
                if (!checkMatchedId.isMatchedCustomer && !checkMatchedId.isAuthorizedAccount)
                {
                    return Forbid();
                }
                var response = await _messageService.GetChatHistoryByCustomerId(CustomerId);
                if(response.CustomerName == null && response.response == null)
                {
                    return BadRequest("Customer does not exist");
                }
                else
                {
                    return Ok(new { customerName = response.CustomerName, messageHistory = response.response });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireStaffRole")]
        [HttpGet("get-chatbox/admin")]
        public async Task<IActionResult> GetChatBoxListForAdmin()
        {
            try
            {
                var response = await _messageService.GetChatBoxList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-chatbox/{CustomerId}")]
        public async Task<IActionResult> GetChatBoxByCustomerId(int CustomerId)
        {
            try
            {
                var response = await _messageService.GetChatBoxByCustomerId(CustomerId);
                if (response == null)
                {
                    return BadRequest("No chat");
                } else
                {
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
