using Application.IServices;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ChatGPTService : IChatGPTService
    {


        public async Task<string> GetChatGPTAnswer(string questionText)
        {
            string chatGPTAPIkey = "";
            string answer = string.Empty;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {chatGPTAPIkey}");

            // Lấy thông tin cấu trúc cơ sở dữ liệu
            string schemaInfo = await GetDatabaseSchemaInfo();

            // Gọi ChatGPT để xác định xem câu hỏi có liên quan đến database không và tạo truy vấn SQL
            var identifyRequestBody = new
            {
                model = "gpt-4-turbo",
                messages = new[]
                {
        new {
            role = "system",
            content = $@"
            Bạn là một trợ lý AI được phát triển đặc biệt để hỗ trợ người dùng trên trang web https://www.goodjobs.works/. 
            Trang web này là một nền tảng tìm kiếm việc làm freelance cho sinh viên tại Việt Nam. 
            Nhiệm vụ của bạn bao gồm việc trả lời các câu hỏi của người dùng, xác định xem câu hỏi có liên quan đến cơ sở dữ liệu hay không, 
            và nếu có, bạn cần phải tạo ra truy vấn SQL dùng trên Azure database phù hợp. Bạn cần phải đưa ra câu trả lời ngắn gọn, chính xác và dễ hiểu. 
            Cấu trúc cơ sở dữ liệu của trang web này là: {schemaInfo}. 
            Hãy trả lời như một trợ lý AI chuyên nghiệp, luôn hướng đến việc giúp đỡ người dùng một cách hiệu quả nhất."
        },
        new {
            role = "user",
            content = $"Câu hỏi sau đây có phải là truy vấn cơ sở dữ liệu không? Nếu có, hãy tạo truy vấn SQL phù hợp. Nếu không, chỉ cần trả lời 'Không'. \"{questionText}\""
        }
    }
            };


            var jsonIdentifyRequestBody = JsonConvert.SerializeObject(identifyRequestBody);
            var identifyContent = new StringContent(jsonIdentifyRequestBody, Encoding.UTF8, "application/json");

            var identifyResponse = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", identifyContent);
            var identifyResponseString = await identifyResponse.Content.ReadAsStringAsync();

            if (identifyResponse.IsSuccessStatusCode)
            {
                var identifyResponseJson = JsonConvert.DeserializeObject<dynamic>(identifyResponseString);
                string chatGptResponse = identifyResponseJson.choices[0].message.content.ToString().Trim();

                if (chatGptResponse.ToLower() != "không")
                {
                    // Nếu ChatGPT trả về một truy vấn SQL, thực hiện truy vấn đó
                    return QueryDatabase(chatGptResponse);
                }
            }
            else
            {
                throw new Exception($"Error: {identifyResponse.StatusCode}, Content: {identifyResponseString}");
            }

            // Nếu không liên quan đến database, thì gọi ChatGPT để lấy câu trả lời
            var chatRequestBody = new
            {
                model = "gpt-4-turbo",
                messages = new[]
                {
            new { role = "user", content = questionText }
        }
            };

            var jsonChatRequestBody = JsonConvert.SerializeObject(chatRequestBody);
            var chatContent = new StringContent(jsonChatRequestBody, Encoding.UTF8, "application/json");

            var chatResponse = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", chatContent);
            var chatResponseString = await chatResponse.Content.ReadAsStringAsync();

            if (chatResponse.IsSuccessStatusCode)
            {
                var chatResponseJson = JsonConvert.DeserializeObject<dynamic>(chatResponseString);
                answer = chatResponseJson.choices[0].message.content.ToString();
            }
            else
            {
                throw new Exception($"Error: {chatResponse.StatusCode}, Content: {chatResponseString}");
            }

            return answer;
        }

        private async Task<string> GetDatabaseSchemaInfo()
        {
            string connectionString = "Data Source=tcp:sv-doan-2.database.windows.net,1433;Initial Catalog=DoAnServer2.0;User Id=anhn2592@sv-doan-2;Password=Leduc2810;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            StringBuilder schemaInfo = new StringBuilder();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // Lấy thông tin về các bảng trong cơ sở dữ liệu
                DataTable tables = conn.GetSchema("Tables");
                foreach (DataRow row in tables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    schemaInfo.AppendLine($"Table: {tableName}");

                    // Lấy thông tin về các cột trong mỗi bảng
                    SqlCommand cmd = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", conn);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        schemaInfo.AppendLine($"    Column: {reader["COLUMN_NAME"]}, Type: {reader["DATA_TYPE"]}");
                    }

                    reader.Close();
                }

                conn.Close();
            }

            return schemaInfo.ToString();
        }

        private string QueryDatabase(string query)
        {
            string connectionString = "Data Source=tcp:sv-doan-2.database.windows.net,1433;Initial Catalog=DoAnServer2.0;User Id=anhn2592@sv-doan-2;Password=Leduc2810;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string result = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Log hoặc in ra câu truy vấn SQL để kiểm tra
                Console.WriteLine("Executing SQL Query: " + query);

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Đọc dữ liệu từ tất cả các cột
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result += reader.GetName(i) + ": " + reader[i].ToString() + "\n";
                    }
                    result += "\n";
                }

                conn.Close();
            }

            return string.IsNullOrEmpty(result) ? "Không tìm thấy kết quả phù hợp trong database." : result;
        }






    }
}
