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
using Newtonsoft.Json;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml;
using OpenAI_API;
using OpenAI_API.Completions;
using OfficeOpenXml.Style;


namespace Application.Services
{
    public class ExportService : IExportService
    {
        private readonly IStatisticService _statisticService;

        public ExportService(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public async Task<Stream> GenerateExcelFileStream(string fileName, bool isChat)
        {
            // Thiết lập LicenseContext cho EPPlus
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // Lấy dữ liệu từ các dịch vụ
            var categoryPieChartData = await _statisticService.GetCategoryPieChartData();
            //var projectPieChartData = await _statisticService.GetProjectPieChartData();
            var userPieChartData = await _statisticService.GetUserPieChartData();
            var newUserData = await _statisticService.GetNewUserData();



            //Data table danh mục và dự án
            DataTable dataTable1 = new DataTable("Báo cáo");
            dataTable1.Columns.AddRange(new DataColumn[]
            {
        new DataColumn("Tên danh mục"),
        new DataColumn("Tổng số dự án", typeof(int))
            });

            // Data table người dùng
            DataTable dataTable2 = new DataTable("Người dùng");
            dataTable2.Columns.AddRange(new DataColumn[]
            {
        new DataColumn("Phân quyền"),
        new DataColumn("Số lượng người dùng khả dụng", typeof(int)),
        new DataColumn("Số lượng người dùng bị chặn", typeof(int))
            });

            // Thêm dữ liệu vào DataTable2
            dataTable2.Rows.Add("Freelancer", userPieChartData.FreelacerCount, userPieChartData.TotalBlockedFreelancer);
            dataTable2.Rows.Add("Nhà tuyển dụng", userPieChartData.RecruiterCount, userPieChartData.TotalBlockedRecruiter);



            // Thêm dữ liệu vào DataTable1
            foreach (var data in categoryPieChartData.OrderByDescending(x => x.TotalProjects))
            {
                dataTable1.Rows.Add(data.CategoryName, data.TotalProjects);
            }



            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh mục và dự án");
                #region worksheet danh mục và dự án

                worksheet.Cells[1, 1].Value = "Báo cáo về thống kê tổng số dự án trên mỗi danh mục";
                worksheet.Cells[1, 1, 1, 20].Merge = true; // Merge các ô từ A1 đến cột cuối cùng của hàng 1
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa
                worksheet.Cells[1, 1].Style.Font.Size = 15; // Chỉnh kích thước chữ
                worksheet.Cells[1, 1].Style.Font.Bold = true; // Tô đậm chữ

                worksheet.Cells[2, 1].Value = $"Ngày tạo: {DateTime.Now}";
                worksheet.Cells[2, 1, 2, 20].Merge = true; // Merge các ô từ A2 đến cột cuối cùng của hàng 2
                worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa
                worksheet.Cells[2, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet.Cells[2, 1].Style.Font.Bold = true; // Tô đậm chữ

                string target;
                string comment;
                string propose;
                // Tạo các Task song song cho các yêu cầu API

                //Data để call api chatgpt
                string dataTableContent = "Danh sách các danh mục và tổng số dự án:\n";
                foreach (DataRow row in dataTable1.Rows)
                {
                    dataTableContent += $"{row["Tên danh mục"]}: {row["Tổng số dự án"]} dự án\n";
                }
                if (isChat)
                {
                    var targetTask = await GetChatGPTAnswer(
                   "Tôi là Admin và Trang web của tôi là về tìm kiếm việc làm freelancer, dựa vào tên báo cáo thống kê này, bạn hãy đưa ra ngắn gọn mục tiêu của báo cáo này giúp tôi: " +
                   worksheet.Cells[1, 1].Value);

                    var commentTask = await GetChatGPTAnswer("Từ nội dung sau, hãy đưa ra kết luận báo cáo thống kê cho tôi, hãy viết ngắn gọn:" + dataTableContent);
                    var proposeTask = await GetChatGPTAnswer("Từ nội dung sau, hãy đưa ra đề xuất để có thể cải thiện cho doanh số trang web, có thể là tạo thêm nhiều blog về các danh mục ít dự án chẳng hạn, hãy viết ngắn gọn:" + dataTableContent);
                    target = targetTask;
                    comment = commentTask;
                    propose = proposeTask;
                }
                else
                {
                    target = "";
                    comment = "";
                    propose = "";
                }


                // Thêm mục tiêu vào worksheet
                worksheet.Cells[4, 1].Value = "Mục tiêu: ";
                worksheet.Cells[4, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[4, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                worksheet.Cells[4, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet.Cells[4, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet.Cells[5, 1].Value = target;
                worksheet.Cells[5, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet.Cells[5, 1].Style.WrapText = true;
                worksheet.Cells[5, 1, 6, 20].Merge = true;
                worksheet.Cells[7, 1].Value = "";

                // Thêm tiêu đề cho bảng
                int startRow = 9; // Hàng bắt đầu vẽ bảng
                int startColumn = 1; // Cột bắt đầu vẽ bảng

                worksheet.Cells[startRow, startColumn].Value = "Tên danh mục";
                worksheet.Cells[startRow, startColumn + 1].Value = "Tổng số dự án";
                var headerRange = worksheet.Cells[startRow, startColumn, startRow, startColumn + 1];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Thêm dữ liệu vào bảng 1
                int currentRow = startRow + 1;
                foreach (DataRow row in dataTable1.Rows)
                {
                    worksheet.Cells[currentRow, startColumn].Value = row["Tên danh mục"];
                    worksheet.Cells[currentRow, startColumn + 1].Value = row["Tổng số dự án"];
                    currentRow++;
                }

                // Tính lại vị trí kết thúc cột và hàng bảng
                int endRow = currentRow - 1;
                int endColumn = startColumn + 1;

                // Format lại bảng
                worksheet.Cells[startRow, startColumn, endRow, endColumn].AutoFitColumns();

                // Thêm các hàng bổ sung
                var maxProjectsRow = categoryPieChartData.OrderByDescending(d => d.TotalProjects).FirstOrDefault();
                string maxProjectsCategory = maxProjectsRow?.CategoryName ?? "N/A";
                int maxProjectsCount = maxProjectsRow?.TotalProjects ?? 0;
                int totalCategory = categoryPieChartData.Count();
                var zeroProjectsCategories = categoryPieChartData
                    .Where(d => d.TotalProjects == 0)
                    .ToList();

                worksheet.Cells[currentRow + 2, 1].Value = "Tóm tắt";
                worksheet.Cells[currentRow + 2, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet.Cells[currentRow + 2, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet.Cells[currentRow + 2, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 2, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[currentRow + 3, 1].Value = $"Tổng số danh mục: {totalCategory} danh mục";
                worksheet.Cells[currentRow + 4, 1].Value = $"Danh mục có nhiều dự án nhất: {maxProjectsCategory} - {maxProjectsCount} dự án";

                worksheet.Cells[currentRow + 6, 1].Value = "Kết luận";
                worksheet.Cells[currentRow + 6, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet.Cells[currentRow + 6, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet.Cells[currentRow + 6, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 6, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells[currentRow + 7, 1].Value = comment;
                worksheet.Cells[currentRow + 7, 1].Style.WrapText = true;
                worksheet.Cells[currentRow + 7, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet.Cells[currentRow + 7, 1, currentRow + 8, 20].Merge = true;

                worksheet.Cells[currentRow + 10, 1].Value = "Đề xuất: ";
                worksheet.Cells[currentRow + 10, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet.Cells[currentRow + 10, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet.Cells[currentRow + 10, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet.Cells[currentRow + 10, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[currentRow + 10, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Khaki);

                worksheet.Cells[currentRow + 11, 1].Value = propose;
                worksheet.Cells[currentRow + 11, 1].Style.WrapText = true;
                worksheet.Cells[currentRow + 11, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet.Cells[currentRow + 11, 1, currentRow + 18, 20].Merge = true;
                worksheet.Cells[startRow, startColumn, endRow, endColumn].AutoFitColumns();
                // Thêm biểu đồ tròn (pie chart)
                var chart = worksheet.Drawings.AddChart("PieChart", eChartType.Pie);
                chart.Title.Text = "Biểu đồ phân phối dự án theo danh mục";
                chart.SetPosition(7, 0, 4, 0); // Vị trí của biểu đồ
                chart.SetSize(800, 400); // Kích thước của biểu đồ

                var series = chart.Series.Add(worksheet.Cells[$"B{startRow + 1}:B{endRow}"], worksheet.Cells[$"A{startRow + 1}:A{endRow}"]);
                series.Header = "Tổng số dự án";
                #endregion


                var worksheet2 = package.Workbook.Worksheets.Add("Người dùng");
                #region worksheet người dùng
                worksheet2.Cells[1, 1].Value = "Báo cáo về người dùng hệ thống";
                worksheet2.Cells[1, 1, 1, 20].Merge = true;
                worksheet2.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells[1, 1].Style.Font.Size = 15;
                worksheet2.Cells[1, 1].Style.Font.Bold = true;

                worksheet2.Cells[2, 1].Value = $"Ngày tạo: {DateTime.Now}";
                worksheet2.Cells[2, 1, 2, 20].Merge = true;
                worksheet2.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet2.Cells[2, 1].Style.Font.Size = 13;
                worksheet2.Cells[2, 1].Style.Font.Bold = true;



                // Lấy kết quả từ các Task
                string target2 ;
                string comment2 ;
                string propose2 ;

                //Data để call api chatgpt
                string dataTableContent2 = "Danh sách người dùng khả dụng và bị chặn theo mỗi phân quyền:\n";
                foreach (DataRow row in dataTable2.Rows)
                {
                    dataTableContent2 += $"{row["Phân quyền"]}: Số lượng người dùng khả dụng: {row["Số lượng người dùng khả dụng"]} người dùng, Số lượng người dùng bị chặn: {row["Số lượng người dùng bị chặn"]} người dùng\n";
                }

                if (isChat)
                {
                    var targetTask2 = await GetChatGPTAnswer2(
                    "Tôi là Admin và Trang web của tôi là về tìm kiếm việc làm freelancer, dựa vào tên báo cáo thống kê này, bạn hãy đưa ra ngắn gọn mục tiêu của báo cáo này giúp tôi: " +
                    worksheet2.Cells[1, 1].Value);

                    var commentTask2 = await GetChatGPTAnswer2("Từ nội dung sau, hãy đưa ra kết luận báo cáo thống kê cho tôi, hãy viết ngắn gọn:" + dataTableContent2);
                    
                    var proposeTask2 = await GetChatGPTAnswer2("Từ nội dung sau, hãy đưa ra đề xuất để có thể cải thiện cho doanh số trang web, số lượng người dùng truy cập có thể là thêm nhiều ưu đãi khi là người mới,..., hãy viết ngắn gọn:" + dataTableContent2);
                    target2 = targetTask2;
                    comment2 = commentTask2;
                    propose2 = proposeTask2;
                }
                else
                {
                    target2 = "";
                    comment2 = "";
                    propose2 = "";
                }


                // Thêm mục tiêu vào worksheet
                worksheet2.Cells[4, 1].Value = "Mục tiêu: ";
                worksheet2.Cells[4, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet2.Cells[4, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                worksheet2.Cells[4, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet2.Cells[4, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet2.Cells[5, 1].Value = target2;
                worksheet2.Cells[5, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet2.Cells[5, 1].Style.WrapText = true;
                worksheet2.Cells[5, 1, 6, 20].Merge = true;
                worksheet2.Cells[7, 1].Value = "";

                // Thêm tiêu đề cho bảng
                int startRow2 = 9; // Hàng bắt đầu vẽ bảng
                int startColumn2 = 1; // Cột bắt đầu vẽ bảng


                worksheet2.Cells[startRow2, startColumn2].Value = "Phân quyền";
                worksheet2.Cells[startRow2, startColumn2 + 1].Value = "Số lượng người dùng khả dụng";
                worksheet2.Cells[startRow2, startColumn2 + 2].Value = "Số lượng người dùng bị chặn";
                var headerRange2 = worksheet2.Cells[startRow2, startColumn2, startRow2, startColumn2 + 2];
                headerRange2.Style.Font.Bold = true;
                headerRange2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Thêm dữ liệu vào bảng 2
                int currentRow2 = startRow2 + 1;

                foreach (DataRow row in dataTable2.Rows)
                {
                    worksheet2.Cells[currentRow2, startColumn2].Value = row["Phân quyền"];
                    worksheet2.Cells[currentRow2, startColumn2 + 1].Value = row["Số lượng người dùng khả dụng"];
                    worksheet2.Cells[currentRow2, startColumn2 + 2].Value = row["Số lượng người dùng bị chặn"];
                    currentRow2++;
                }

                int endRow2 = currentRow2 - 1;
                int endColumn2 = startColumn2 + 1;



                // Thêm các hàng bổ sung
                var totalUser = userPieChartData.TotalUser;
                var totalBlockedUser = userPieChartData.TotalBlockedFreelancer + userPieChartData.TotalBlockedRecruiter;


                worksheet2.Cells[currentRow2 + 2, 1].Value = "Tóm tắt";
                worksheet2.Cells[currentRow2 + 2, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet2.Cells[currentRow2 + 2, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet2.Cells[currentRow2 + 2, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet2.Cells[currentRow2 + 2, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet2.Cells[currentRow2 + 3, 1].Value = $"Tổng số người dùng: {totalUser} người dùng";
                worksheet2.Cells[currentRow2 + 4, 1].Value = $"Tổng số người dùng bị chặn: {totalBlockedUser} người dùng";

                worksheet2.Cells[currentRow2 + 6, 1].Value = "Kết luận";
                worksheet2.Cells[currentRow2 + 6, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet2.Cells[currentRow2 + 6, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet2.Cells[currentRow2 + 6, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet2.Cells[currentRow2 + 6, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet2.Cells[currentRow2 + 7, 1].Value = comment2;
                worksheet2.Cells[currentRow2 + 7, 1].Style.WrapText = true;
                worksheet2.Cells[currentRow2 + 7, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet2.Cells[currentRow2 + 7, 1, currentRow2 + 8, 20].Merge = true;

                worksheet2.Cells[currentRow2 + 10, 1].Value = "Đề xuất: ";
                worksheet2.Cells[currentRow2 + 10, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet2.Cells[currentRow2 + 10, 1].Style.Font.Size = 13; // Chỉnh kích thước chữ
                worksheet2.Cells[currentRow2 + 10, 1].Style.Font.Italic = true; // Tô đậm chữ
                worksheet2.Cells[currentRow2 + 10, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet2.Cells[currentRow2 + 10, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Khaki);

                worksheet2.Cells[currentRow2 + 11, 1].Value = propose2;
                worksheet2.Cells[currentRow2 + 11, 1].Style.WrapText = true;
                worksheet2.Cells[currentRow2 + 11, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                worksheet2.Cells[currentRow2 + 11, 1, currentRow2 + 18, 20].Merge = true;
                worksheet2.Cells[startRow2, startColumn2, endRow2, endColumn2].AutoFitColumns();

                // Thêm biểu đồ tròn (pie chart) cho người dùng
                var chart2 = worksheet2.Drawings.AddChart("UserPieChart", eChartType.Pie);
                chart2.Title.Text = "Biểu đồ phân phối người dùng theo phân quyền";
                chart2.SetPosition(7, 0, 6, 0);
                chart2.SetSize(300, 175);

                var series2 = chart2.Series.Add(worksheet2.Cells[$"B{startRow2 + 1}:B{endRow2}"], worksheet2.Cells[$"A{startRow2 + 1}:A{endRow2}"]);
                series2.Header = "Số lượng người dùng khả dụng và bị chặn mỗi phân quyền";

                #endregion

                #region Doanh thu
                #endregion


                // Tạo MemoryStream để lưu file
                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);
                memoryStream.Position = 0; // Reset vị trí của stream
                return memoryStream;
            }
        }




        public async Task<string> GetChatGPTAnswer(string questionText)
        {
            var chatGPTAPIkey = "";
            string answer = string.Empty;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {chatGPTAPIkey}");

            var requestBody = new
            {
                model = "gpt-4-turbo",
                messages = new[]
                {
                    new { role = "user", content = questionText }
                }
            };

            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                answer = responseJson.choices[0].message.content.ToString();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, Content: {responseString}");
            }

            return answer;

        }

        public async Task<string> GetChatGPTAnswer2(string questionText)
        {

            var chatGPTAPIkey ="";
            string answer = string.Empty;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {chatGPTAPIkey}");

            var requestBody = new
            {
                model = "gpt-4-turbo",
                messages = new[]
                {
                    new { role = "user", content = questionText }
                }
            };

            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                answer = responseJson.choices[0].message.content.ToString();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, Content: {responseString}");
            }

            return answer;


        }

        public async Task<string> GetChatGPTAnswer3(string questionText)
        {

            var chatGPTAPIkey = "";
            string answer = string.Empty;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {chatGPTAPIkey}");

            var requestBody = new
            {
                model = "gpt-4-turbo",
                messages = new[]
                {
                    new { role = "user", content = questionText }
                }
            };

            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                answer = responseJson.choices[0].message.content.ToString();
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode}, Content: {responseString}");
            }

            return answer;

        }
    }
}
