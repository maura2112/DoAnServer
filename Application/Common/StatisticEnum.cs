using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class StatisticEnum
    {
        public enum UserStatistic
        {
            [Description("Tổng số đánh giá tích cực")]
            PositiveRating = 1,
            [Description("Tổng số đánh giá tiêu cực")]
            NegativeRating = 2
        }
        public enum SkillStatistic
        {
            [Description("Tổng số dự án đã được đăng với kĩ năng này")]
            TotalPostedProjects = 1,
            [Description("Tổng số người dùng với kĩ năng này")]
            TotalUsers = 2
        }
    }
}
