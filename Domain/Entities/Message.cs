using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


public partial class Message
{
    public int MessagesId { get; set; }
    public int? ConversationId { get; set; }
    public int? SenderId { get; set; }
    public string? MessageText { get; set; }
    public DateTime? SendDate { get; set; }
    public int? IsRead { get; set; }
    public int? MessageType { get; set; }
    public string? File {  get; set; }

    [JsonIgnore]
    public virtual Conversation? Conversation { get; set; }
    [JsonIgnore]

    public virtual AppUser? Sender { get; set; }
}

