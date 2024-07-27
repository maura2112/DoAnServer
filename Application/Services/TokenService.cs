using Application.IServices;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IMemoryCache _memoryCache;

        public TokenService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Lưu mã xác nhận với token vào bộ nhớ cache
        public void SaveToken(string token, string code)
        {
            // Cài đặt thời gian sống của token (ví dụ: 15 phút)
            var expirationTime = TimeSpan.FromMinutes(15);

            _memoryCache.Set(token, code, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            });
        }

        // Lấy mã xác nhận dựa trên token từ bộ nhớ cache
        public string GetCodeByToken(string token)
        {
            if (_memoryCache.TryGetValue(token, out string code))
            {
                return code;
            }

            return null;
        }
    }
}
