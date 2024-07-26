using API.Hubs;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using Application.Services;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    public class ExportController : ApiControllerBase
    {
        private readonly IExportService _exportService;

        public ExportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet]
        [Route(Common.Url.Export.ExportStatistic)]
        public async Task<IActionResult> GenerateExcel(bool isChat)
        {
            var currentDate = DateTime.Now;
            var fileName = $"Báo cáo thống kê ({DateTimeHelper.ToVietnameseOnlyDateString(currentDate)}).xlsx";
            var fileStream = await _exportService.GenerateExcelFileStream(fileName, isChat);

            if (fileStream == null)
            {
                return StatusCode(500, new { message = "Failed to generate the file." });
            }

            // Trả về file như một phản hồi HTTP
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        //[HttpPost]
        //[Route(Common.Url.Export.AskingChatGPT)]
        //public async Task<IActionResult> AskChat(string question)
        //{
        //    var chat = await _exportService.GetChatGPTAnswer(question);
        //    return Ok(new
        //    {
        //        message = chat
        //    });
        //}
    }
}
