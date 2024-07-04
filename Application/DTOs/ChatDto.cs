using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ChatDto
    {
        public int MessagesId { get; set; }
        public int? ConversationId { get; set; }
        public int? SenderId { get; set; }
        public string? MessageText { get; set; }
        public DateTime? SendDate { get; set; }
        public int? IsRead { get; set; }
        public int? MessageType { get; set; }
        public string? File { get; set; }
    }
}
