using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IServices;
using ClosedXML.Excel;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml;

namespace Application.Services
{
    public class ExportService : IExportService
    {
        private readonly IStatisticService _statisticService;

        public ExportService(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public async Task<string> GenerateExcelFilePath(string fileName)
        {
            // Thiết lập LicenseContext cho EPPlus
            ExcelPackage.LicenseContext = LicenseContext.Commercial; // Hoặc LicenseContext.NonCommercial nếu bạn đang sử dụng giấy phép phi thương mại

            var categoryPieChartData = await _statisticService.GetCategoryPieChartData();
            var projectPieChartData = await _statisticService.GetProjectPieChartData();
            var userPieChartData = await _statisticService.GetUserPieChartData();
            var newUserData = await _statisticService.GetNewUserData();

            // Tạo DataTable để định nghĩa cấu trúc dữ liệu
            DataTable dataTable1 = new DataTable("Báo cáo");
            dataTable1.Columns.AddRange(new DataColumn[]
            {
        new DataColumn("Tên danh mục"),
        new DataColumn("Tổng số dự án", typeof(int))
            });

            // Thêm dữ liệu vào DataTable
            foreach (var data in categoryPieChartData.OrderByDescending(x => x.TotalProjects))
            {
                dataTable1.Rows.Add(data.CategoryName, data.TotalProjects);
            }

            // Định nghĩa đường dẫn lưu file
            var desktopPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                fileName);

            // Tạo file Excel và worksheet
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm các hàng tiêu đề vào worksheet
                worksheet.Cells[1, 1].Value = "Báo cáo về thống kê tổng số dự án trên mỗi danh mục";
                worksheet.Cells[2, 1].Value = $"Ngày tạo: {DateTime.Now}";
                worksheet.Cells[3, 1].Value = "Mục tiêu: Đánh giá số lượng dự án theo các danh mục để xác định xu hướng và các danh mục cần chú ý";
                worksheet.Cells[4, 1].Value = ""; // Ô trống nếu không cần dữ liệu

                // Thêm tiêu đề cột vào worksheet
                worksheet.Cells[5, 1].Value = "Tên danh mục";
                worksheet.Cells[5, 2].Value = "Tổng số dự án";

                // Thêm dữ liệu vào worksheet
                int currentRow = 6; // Dữ liệu bắt đầu từ hàng 6
                foreach (DataRow row in dataTable1.Rows)
                {
                    worksheet.Cells[currentRow, 1].Value = row["Tên danh mục"];
                    worksheet.Cells[currentRow, 2].Value = row["Tổng số dự án"];
                    currentRow++;
                }

                var maxRowCountForChart = currentRow;

                // Thêm các hàng bổ sung
                var maxProjectsRow = categoryPieChartData.OrderByDescending(d => d.TotalProjects).FirstOrDefault();
                string maxProjectsCategory = maxProjectsRow?.CategoryName ?? "N/A";
                int maxProjectsCount = maxProjectsRow?.TotalProjects ?? 0;
                int totalCategory = categoryPieChartData.Count();
                var zeroProjectsCategories = categoryPieChartData
                    .Where(d => d.TotalProjects == 0)
                    .ToList();

                worksheet.Cells[currentRow + 2, 1].Value = "Tóm tắt";
                worksheet.Cells[currentRow + 2, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 2, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[currentRow + 3, 1].Value = $"Tổng số danh mục: {totalCategory} danh mục";
                worksheet.Cells[currentRow + 3, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 3, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Khaki);

                worksheet.Cells[currentRow + 4, 1].Value = $"Danh mục có nhiều dự án nhất: {maxProjectsCategory} - {maxProjectsCount} dự án";
                worksheet.Cells[currentRow + 4, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 4, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);

                worksheet.Cells[currentRow + 6, 1].Value = "Kết luận";
                worksheet.Cells[currentRow + 6, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 6, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[currentRow + 7, 1].Value = $"Báo cáo chỉ ra rằng danh mục {maxProjectsCategory} có số lượng dự án cao nhất, trong khi các danh mục dưới đây chưa có dự án nào:";

                worksheet.Cells[currentRow + 8, 1].Value = "Những danh mục cần cân nhắc";
                worksheet.Cells[currentRow + 8, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 8, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                currentRow += 9; // Bỏ qua các hàng tiêu đề và dữ liệu trước đó
                foreach (var category in zeroProjectsCategories)
                {
                    worksheet.Cells[currentRow, 1].Value = category.CategoryName;
                    worksheet.Cells[currentRow, 2].Value = category.TotalProjects;
                    currentRow++;
                }

                worksheet.Cells[currentRow, 1].Value = "Đề xuất: Tạo thêm Blog cho các danh mục trên, hoặc sau 1 tháng báo cáo lại, nếu vẫn chưa có dự án nào thì xem xét đến việc xóa danh mục trên ";
                worksheet.Cells[currentRow, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Khaki);

                // Thêm biểu đồ tròn (pie chart)
                var chart = worksheet.Drawings.AddChart("PieChart", eChartType.Pie);
                chart.Title.Text = "Biểu đồ phân phối dự án theo danh mục";
                chart.SetPosition(5, 0, 4, 0); // Vị trí của biểu đồ
                chart.SetSize(450, 300); // Kích thước của biểu đồ

                var series = chart.Series.Add(worksheet.Cells[$"B6:B{maxRowCountForChart - 1}"], worksheet.Cells[$"A6:A{maxRowCountForChart - 1}"]);
                series.Header = "Tổng số dự án";

                // Lưu file
                var fileInfo = new FileInfo(desktopPath);
                package.SaveAs(fileInfo);

                return desktopPath;
            }
        }



    }
}
