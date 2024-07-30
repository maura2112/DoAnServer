
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Statistic;
using Domain.Common;

namespace Application.IServices
{
    public interface IStatisticService
    {
        //chart
        public Task<List<CategoriesPieChart>> GetCategoryPieChartData();
        public Task<List<CategoriesPieChart>> GetCategoryPieChartExport();
        public Task<ProjectsPieChart> GetProjectPieChartData();
        public Task<UsersPieChart> GetUserPieChartData();
        public Task<UsersPieChart> GetUserPieChartExport();
        public Task<List<NewUser>> GetNewUserData();

        //datagrid
        public Task<Pagination<StatisticProjects>> GetProjectStatisticData(int pageIndex, int pageSize);
        public Task<Pagination<StatisticUsers>> GetUserStatisticData( int pageIndex, int pageSize);
        public Task<Pagination<StatisticSkills>> GetSkillStatisticData( int pageIndex, int pageSize);

    }
}
