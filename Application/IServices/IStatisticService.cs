
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Statistic;

namespace Application.IServices
{
    public interface IStatisticService
    {
        public Task<List<CategoriesPieChart>> GetCategoryPieChartData();
        public Task<ProjectsPieChart> GetProjectPieChartData();
        public Task<UsersPieChart> GetUserPieChartData();
        public Task<List<NewUser>> GetNewUserData();

    }
}
