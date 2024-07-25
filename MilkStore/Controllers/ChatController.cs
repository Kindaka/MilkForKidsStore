using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkStore_BAL.ModelViews.ChatDTOs;
using MilkStore_BAL.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MilkStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-room")]
        public async Task<IActionResult> CreateRoom([FromBody] string roomId)
        {
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            await _chatService.CreateRoomAsync(roomId);
            return Ok(new { message = "Chat room created", roomId });
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            var accountIdClaim = User.FindFirst("AccountId");
            if (accountIdClaim == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var accountId = int.Parse(accountIdClaim.Value);
            message.UserId = accountId.ToString(); // Gán AccountId vào UserId của tin nhắn
            await _chatService.SendMessageAsync(message);
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-messages/{roomId}")]
        public async Task<IActionResult> GetMessages(string roomId)
        {
            var messages = await _chatService.GetMessagesAsync(roomId);
            return Ok(messages);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _chatService.GetAllRoomsAsync();
            return Ok(rooms);
        }


    }
}
