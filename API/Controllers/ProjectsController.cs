using Application.DTOs;
using Application.IServices;

using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{

    public class ProjectsController : ApiControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route(Common.Url.Project.GetAll)]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            return Ok(await _projectService.Get(pageIndex, pageSize));
        }

        [HttpGet]
        [Route(Common.Url.Project.GetByCategory)]
        public async Task<IActionResult> GetByCate([FromQuery] ProjectSearchDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Project, bool>> filter = null;
            if (projects != null)
            {
                filter = item => item.CategoryId == projects.CategoryId;
            }
            return Ok(await _projectService.GetWithFilter(filter, projects.PageIndex, projects.PageSize));
        }
        
        [HttpGet]
        [Route(Common.Url.Project.GetProjectDetails)]
        public async Task<IActionResult> GetDetailProject(int id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            return Ok(await _projectService.GetDetailProjectById(id));
        }

        [HttpPost]
        [Route(Common.Url.Project.Add)]
        public async Task<IActionResult> AddAsync([FromQuery] ProjectDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            await _projectService.Add(DTOs);
            return NoContent();
        }

    }
}
