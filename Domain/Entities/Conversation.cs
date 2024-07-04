using Domain.Entities;
using System;
using System.Collections.Generic;


    public partial class Conversation
    {

        public int ConversationId { get; set; }
        public int? User1 { get; set; }
        public int? User2 { get; set; }

        public virtual AppUser? User1Navigation { get; set; }
        public virtual AppUser? User2Navigation { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }

