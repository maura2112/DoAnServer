using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class TimeAgoHelper
    {
        public static string CalculateTimeAgo(DateTime createdDate)
        {
            var timeSpan = DateTime.UtcNow - createdDate;

            if (timeSpan.TotalSeconds < 60)
                return $"{timeSpan.Seconds} giây trước";
            else if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.Minutes} phút trước";
            else if (timeSpan.TotalHours < 24)
                return $"{timeSpan.Hours} giờ trước";
            else if (timeSpan.TotalDays < 30)
                return $"{timeSpan.Days} ngày trước";
            else if (timeSpan.TotalDays < 365)
                return $"{timeSpan.Days / 30} tháng trước";
            else
                return $"{timeSpan.Days / 365} năm trước";
        }
    }
}
