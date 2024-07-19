using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_MilkForKidsStore.DTO.ChatDTOs
{
    public class ChatMessage
    {
        public string RoomId { get; set; }
        public string UserId { get; set; } // This will be AccountId
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
