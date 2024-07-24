using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.ModelViews.ChatDTOs
{
    public class MessageListDtoResponse
    {
        public string? CustomerName { get; set; }
        public MessageDtoResponse? response { get; set; }
    }
}
