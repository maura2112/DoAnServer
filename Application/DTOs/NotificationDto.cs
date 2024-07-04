using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int? SendId { get; set; }
        public string? SendUserName { get; set; }
        public string? ProjectName { get; set; }
        public int? RecieveId { get; set; }
        public string? Description { get; set; }
        public DateTime? Datetime { get; set; }
        public int? NotificationType { get; set; }
        public int? IsRead { get; set; }
        public string? Link { get; set; }
    }
}
