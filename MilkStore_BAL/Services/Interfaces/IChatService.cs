using MilkStore_BAL.ModelViews.ChatDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Interfaces
{
    public interface IChatService
    {
        Task<bool> RoomExistsAsync(string roomId);
        Task<List<string>> GetAllRoomsAsync();
        Task<bool> IsAdminAsync(int accountId);
        Task<List<ChatMessage>> GetMessagesAsync(string roomId);
        Task SendMessageAsync(ChatMessage message);
        Task CreateRoomAsync(string roomId);

    }
}
