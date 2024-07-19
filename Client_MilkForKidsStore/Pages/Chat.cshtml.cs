using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client_MilkForKidsStore.DTO.ChatDTOs;
using Firebase.Database.Query;

namespace Client_MilkForKidsStore.Pages
{
    public class ChatModel : PageModel
    {
        private readonly ILogger<ChatModel> _logger;
        private readonly FirebaseClient _firebaseClient;

        public ChatModel(ILogger<ChatModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _firebaseClient = new FirebaseClient(configuration["Firebase:Chat:DatabaseUrl"]);
        }

        [BindProperty]
        public string RoomId { get; set; }
        [BindProperty]
        public string Message { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public List<string> Rooms { get; set; } = new List<string>();

        public async Task OnGetAsync(string roomId)
        {
            RoomId = roomId;
            Messages = await GetMessagesAsync(roomId);
            Rooms = await GetRoomsAsync();
        }

        public async Task<IActionResult> OnPostSendMessageAsync()
        {
            var chatMessage = new ChatMessage
            {
                RoomId = RoomId,
                Message = Message,
                CreatedAt = DateTime.Now
            };

            await _firebaseClient.Child("chatrooms").Child(RoomId).Child("messages").PostAsync(chatMessage);

            return RedirectToPage(new { roomId = RoomId });
        }

        public async Task<IActionResult> OnPostCreateRoomAsync()
        {
            await _firebaseClient.Child("chatrooms").Child(RoomId).PutAsync(new { createdAt = DateTime.Now });

            return RedirectToPage(new { roomId = RoomId });
        }

        private async Task<List<ChatMessage>> GetMessagesAsync(string roomId)
        {
            var messages = await _firebaseClient.Child("chatrooms").Child(roomId).Child("messages").OnceAsync<ChatMessage>();
            var result = new List<ChatMessage>();
            foreach (var msg in messages)
            {
                result.Add(msg.Object);
            }
            return result;
        }

        private async Task<List<string>> GetRoomsAsync()
        {
            var rooms = await _firebaseClient.Child("chatrooms").OnceAsync<object>();
            var result = new List<string>();
            foreach (var room in rooms)
            {
                result.Add(room.Key);
            }
            return result;
        }
    }
}
