using API.Hubs;
using Application.DTOs;
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
        public async Task<IActionResult> GenerateExcel()
        {
            var fileName = "Statistic.xlsx";
            var filePath = await _exportService.GenerateExcelFilePath(fileName);

            if (System.IO.File.Exists(filePath))
            {
                return Ok(new { message = $"File has been saved to {filePath}" });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to save the file." });
            }
        }
    }
}
