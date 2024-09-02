using API.Hubs;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Threading;

namespace API.Controllers
{
    public class ExportController : ApiControllerBase
    {
        private readonly IExportService _exportService;
        private readonly IChatGPTService _chatGPTService;
        private ApplicationDbContext _context = new ApplicationDbContext();
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly ICurrentUserService _currentUserService;

        public ExportController(IExportService exportService, IChatGPTService chatGPTService, IMapper mapper, IHubContext<ChatHub> chatHubContext, ICurrentUserService currentUserService)
        {
            _exportService = exportService;
            _chatGPTService = chatGPTService;
            _mapper = mapper;
            _chatHubContext = chatHubContext;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Route(Common.Url.Export.ExportStatistic)]
        public async Task<IActionResult> GenerateExcel(bool isChat)
        {
            var currentDate = DateTime.UtcNow;
            var fileName = $"Báo cáo thống kê ({DateTimeHelper.ToVietnameseOnlyDateString(currentDate)}).xlsx";
            var fileStream = await _exportService.GenerateExcelFileStream(fileName, isChat);

            if (fileStream == null)
            {
                return StatusCode(500, new { message = "Failed to generate the file." });
            }

            // Trả về file như một phản hồi HTTP
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        [HttpPost]
        [Route(Common.Url.Export.AskingChatGPT)]
        public async Task<IActionResult> AskChat(ChatDto chatDto)
        {
            Message message = _mapper.Map<Message>(chatDto);
            message.SendDate = DateTime.UtcNow;
            message.IsRead = 1;
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();

            var answer = await _chatGPTService.GetChatGPTAnswer(chatDto.MessageText);

            ChatDto newResponseMessage = new ChatDto
            {
                ConversationId = chatDto.ConversationId,
                MessageText = answer,
                SenderId = 72,
                SendDate = DateTime.UtcNow,
                IsRead = 1,
                MessageType = 1
            };

            Message messageN = _mapper.Map<Message>(newResponseMessage);

            await _context.AddAsync(messageN);
            await _context.SaveChangesAsync();

            var hubConnectionsd = await _context.HubConnections.Where(con => con.userId == chatDto.SenderId).ToListAsync();

            foreach (var hubConnection in hubConnectionsd)
            {
                await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedMessage", messageN);
            }
            foreach (var hubConnection in hubConnectionsd)
            {
                await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("HaveMessage", 1);
            }

            //var hubConnectionsd = await _context.HubConnections.Where(con => con.userId == newResponseMessage.SenderId).ToListAsync();
            //foreach (var hubConnection in hubConnectionsd)
            //{
            //    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedMessage", messageN);
            //}
            //var hubConnections = await _context.HubConnections.Where(con => con.userId == chatDto.SenderId).ToListAsync();
            //foreach (var hubConnection in hubConnections)
            //{
            //    await _chatHubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("HaveMessage", 1);
            //}

            return Ok();
        }

        [HttpPost]
        [Route(Common.Url.Export.SensitiveWord)]
        public async Task<IActionResult> SensitiveWord(string question)
        {
            var answer = await _exportService.SensitiveWord(question);
            return Ok(answer);
        }
    }
}
