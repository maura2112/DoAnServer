using API.Hubs;
using Application.DTOs;
using Application.IServices;
using Application.Services;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel;
using System.Linq.Expressions;
using Google.Apis.Util;
using static Application.Common.StatisticEnum;

namespace API.Controllers
{
    public class StatisticController : ApiControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet]
        [Route(Common.Url.Statistic.CategoriesPieChart)]
        public async Task<IActionResult> GetCategoryPieChartData()
        {
                var result = await _statisticService.GetCategoryPieChartData();
                return Ok(new
                {
                    success = true,
                    data = result
                });
            
        }
        [HttpGet]
        [Route(Common.Url.Statistic.ProjectsPieChart)]
        public async Task<IActionResult> GetProjectPieChartData()
        {
            var result = await _statisticService.GetProjectPieChartData();
            return Ok(new
            {
                success = true,
                data = result
            });
        }
        [HttpGet]
        [Route(Common.Url.Statistic.UsersPieChart)]
        public async Task<IActionResult> GetUserPieChartData()
        {
            var result = await _statisticService.GetUserPieChartData();
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet]
        [Route(Common.Url.Statistic.NewUserData)]
        public async Task<IActionResult> GetNewUserData()
        {
            var result = await _statisticService.GetNewUserData();
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet]
        [Route(Common.Url.Statistic.StatisticUsers)]
        public async Task<IActionResult> GetUserStatisticData( int pageIndex, int pageSize)
        {
            var result = await _statisticService.GetUserStatisticData(pageIndex, pageSize);
            return Ok(new
            {
                success = true,
                data = result
            });

        }
        [HttpGet]
        [Route(Common.Url.Statistic.StatisticSkills)]
        public async Task<IActionResult> GetSkillStatisticData( int pageIndex, int pageSize)
        {
            var result = await _statisticService.GetSkillStatisticData( pageIndex, pageSize);

            

            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpGet]
        [Route(Common.Url.Statistic.StatisticProjects)]
        public async Task<IActionResult> GetProjectStatisticData(int pageIndex, int pageSize)
        {
            var result = await _statisticService.GetProjectStatisticData(pageIndex, pageSize);
            return Ok(new
            {
                success = true,
                data = result
            });

        }
    }
}
