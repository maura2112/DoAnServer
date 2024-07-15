
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
        public Task<ProjectsPieChart> GetProjectPieChartData();
        public Task<UsersPieChart> GetUserPieChartData();
        public Task<List<NewUser>> GetNewUserData();

        //datagrid
        public Task<Pagination<StatisticProjects>> GetProjectStatisticData(int pageIndex, int pageSize);
        public Task<Pagination<StatisticUsers>> GetUserStatisticData(int type, int pageIndex, int pageSize);
        public Task<Pagination<StatisticSkills>> GetSkillStatisticData(int type, int pageIndex, int pageSize);

    }
}
