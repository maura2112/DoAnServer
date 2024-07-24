using API.Hubs;
using Application.DTOs;
using Application.IServices;
using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static API.Common.Url;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly ICurrentUserService _currentUserService;

        public ChatController(IMapper mapper, IHubContext<ChatHub> chatHubContext, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _chatHubContext = chatHubContext;
            _currentUserService = currentUserService;
        }

        [HttpGet("messages/{conversationId}/{pageIndex}")]
        public async Task<IActionResult> GetMessageByConversation(int conversationId, int pageIndex)
        {
            var userId = _currentUserService.UserId;
            var conversation = await _context.Conversations.FirstOrDefaultAsync(x => x.ConversationId == conversationId);
            if (conversation.User1 == userId || conversation.User2 == userId)
            {
                //them page index
                //var list = new Pagination<Message>();
                var list = await _context.Messages
             .Where(x => x.ConversationId == conversationId)
             .OrderByDescending(x => x.SendDate)
             .AsNoTracking()
             .ToListAsync();

                var totalItem = list.Skip((pageIndex - 1) * 10)
                   .Take(10).OrderBy(x => x.SendDate).ToList();

                var result = new Pagination<Message>()
                {
                    TotalItemsCount = list.Count,
                    Items = totalItem,
                    PageIndex = pageIndex,
                    PageSize = 10
                };
                return Ok(result);
            }
            else
            {
                return BadRequest(new
                {
                    message = "Bạn không có quyền vào đoạn chat này"
                });
            }


        }

        [HttpPost("AddConversation/{user1}/{user2}")]
        public async Task<IActionResult> CreateConversation(int user1, int user2)
        {
            AppUser u1 = await _context.Users.FirstOrDefaultAsync(x => x.Id == user1);
            AppUser u2 = await _context.Users.FirstOrDefaultAsync(x => x.Id == user2);

            if (u1 == null || u2 == null)
            {
                return BadRequest();
            }

            var existingConversation = await _context.Conversations
    .FirstOrDefaultAsync(c =>
        (c.User1 == user1 && c.User2 == user2) ||
        (c.User1 == user2 && c.User2 == user1));

            if (existingConversation != null)
            {
                return Ok(existingConversation.ConversationId);
            }

            var conversation = new Conversation()
            {
                User1 = user1,
                User2 = user2
            };

            await _context.AddAsync(conversation);
            await _context.SaveChangesAsync();

            return Ok(conversation.ConversationId);
        }

        [HttpGet("Info/{userId}")]
        public async Task<IActionResult> GetInfo(int userId)
        {
            AppUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return Ok(new
            {
                UserId = user.Id,
                Name = user.Name,
                Avatar = user.Avatar
            });
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(ChatDto chatDto)
        {
            Message message = _mapper.Map<Message>(chatDto);
            message.SendDate = DateTime.Now;
            message.IsRead = 0;

            await _context.AddAsync(message);
            await _context.SaveChangesAsync();

            var conversation = await _context.Conversations
          .Include(c => c.User1Navigation)
          .Include(c => c.User2Navigation)
          .FirstOrDefaultAsync(c => c.ConversationId == chatDto.ConversationId);
            if (chatDto.SenderId == conversation.User1Navigation.Id)
            {
                var hubConnectionsd = await _context.HubConnections.Where(con => con.userId == conversation.User2Navigation.Id).ToListAsync();
                foreach (var hubConnection in hubConnectionsd)
                {
                    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedMessage", message);
                }
                var hubConnections = await _context.HubConnections.Where(con => con.userId == conversation.User1Navigation.Id).ToListAsync();
                foreach (var hubConnection in hubConnections)
                {
                    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("HaveMessage", 1);
                }
            }
            else
            {
                var hubConnections = await _context.HubConnections.Where(con => con.userId == conversation.User1Navigation.Id).ToListAsync();
                foreach (var hubConnection in hubConnections)
                {
                    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedMessage", message);
                }
                var hubConnections2 = await _context.HubConnections.Where(con => con.userId == conversation.User2Navigation.Id).ToListAsync();
                foreach (var hubConnection in hubConnections2)
                {
                    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("HaveMessage", 1);
                }
            }







            return Ok();
        }


        [HttpGet("GetUserConnect/{userId}")]
        public async Task<IActionResult> GetUserConnect(int userId)
        {
            var conversations = await _context.Conversations
                .Include(c => c.Messages)
                .Include(c => c.User1Navigation)
                .Include(c => c.User2Navigation)
                .Where(c => c.User1 == userId || c.User2 == userId)
                .ToListAsync();

            // Extract latest messages and corresponding user details
            var latestMessagesWithUsers = conversations
                .Select(c => new
                {
                    Conversation = c,
                    LatestMessage = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault()
                })
                .Where(x => x.LatestMessage != null)
                .Select(x => new
                {
                    x.LatestMessage.ConversationId,
                    x.LatestMessage.MessageText,
                    x.LatestMessage.SendDate,
                    x.LatestMessage.IsRead,
                    x.LatestMessage.MessageType,
                    x.LatestMessage.File,
                    x.LatestMessage.SenderId,
                    //User = _context.Users.FirstOrDefault(y => y.Id == x.LatestMessage.SenderId )
                    User = x.Conversation.User1 == userId ? x.Conversation.User2Navigation : x.Conversation.User1Navigation
                })
                .Select(x => new
                {
                    x.ConversationId,
                    x.MessageText,
                    x.SendDate,
                    x.IsRead,
                    x.SenderId,
                    x.MessageType,
                    x.File,
                    UserId = x.User.Id,
                    UserName = x.User.Name,
                    Avatar = x.User.Avatar,
                    UserEmail = x.User.Email // Include additional user properties as needed
                })
                .OrderByDescending(x => x.SendDate)
                .ToList();

            return Ok(latestMessagesWithUsers);
        }


        [HttpGet("GetNumberMessage/{userId}")]
        public async Task<IActionResult> GetNumberMessage(int userId)
        {
            var conversations = await _context.Conversations
              .Include(c => c.Messages)
              .Include(c => c.User1Navigation)
              .Include(c => c.User2Navigation)
              .Where(c => c.User1 == userId || c.User2 == userId)
              .ToListAsync();

            // Extract latest messages and corresponding user details
            var latestMessagesWithUsers = conversations
                .Select(c => new
                {
                    Conversation = c,
                    LatestMessage = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault()
                })
                .Where(x => x.LatestMessage != null)
                .Select(x => new
                {
                    x.LatestMessage.ConversationId,
                    x.LatestMessage.MessageText,
                    x.LatestMessage.SendDate,
                    x.LatestMessage.IsRead,
                    x.LatestMessage.MessageType,
                    x.LatestMessage.File,
                    x.LatestMessage.SenderId,
                    //User = _context.Users.FirstOrDefault(y => y.Id == x.LatestMessage.SenderId )
                    User = x.Conversation.User1 == userId ? x.Conversation.User2Navigation : x.Conversation.User1Navigation
                })
                .Select(x => new
                {
                    x.ConversationId,
                    x.MessageText,
                    x.SendDate,
                    x.IsRead,
                    x.SenderId,
                    x.MessageType,
                    x.File,
                    UserId = x.User.Id,
                    UserName = x.User.UserName,
                    Avatar = x.User.Avatar,
                    UserEmail = x.User.Email // Include additional user properties as needed
                })
                .OrderByDescending(x => x.SendDate)
                .ToList();
            int x = 0;
            for (int i = 0; i < latestMessagesWithUsers.Count; i++)
            {
                if (latestMessagesWithUsers[i].IsRead == 0 && latestMessagesWithUsers[i].SenderId != userId)
                {
                    x++;
                }
            }

            return Ok(x);
        }

        [HttpPut("markToRead/{conversationId}")]
        public async Task<IActionResult> MarkToRead(int conversationId)
        {
            var message = await _context.Messages
                                            .Where(x => x.ConversationId == conversationId)
                                            .OrderByDescending(x => x.SendDate)
                                            .FirstOrDefaultAsync();
            if (message == null)
            {
                return NotFound();
            }
            message.IsRead = 1;
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
