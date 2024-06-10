using API.Utilities;
using Application.DTOs;
using Application.IServices;

using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static API.Common.Url;

namespace API.Controllers
{

    public class ProjectsController : ApiControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISkillService _skillService;
        private readonly IProjectRepository _projectRepository;
        public ProjectsController(IProjectService projectService, ICurrentUserService currentUserService, ISkillService skillService, IProjectRepository projectRepository)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
            _skillService = skillService;
            _projectRepository = projectRepository;

        }

        [HttpGet]
        [Route(Common.Url.Project.GetAll)]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            if (pageIndex < 1 || pageSize < 1)
            {
                return BadRequest(new { message = "Số trang hoặc kích cỡ trang lớn hơn 1" });
            }
            else
            {
                return Ok(await _projectService.Get(pageIndex, pageSize));
            }

        }

        [HttpGet]
        [Route(Common.Url.Project.Search)]
        public async Task<IActionResult> Search([FromQuery] ProjectSearchDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Domain.Entities.Project, bool>> filter = null;
            if (projects != null && !string.IsNullOrEmpty(projects.Keyword))
            {
                var keyword = projects.Keyword.ToLower();
                filter = item => item.Title.ToLower().Contains(keyword);
            }
            return Ok(await _projectService.GetWithFilter(filter, projects.PageIndex, projects.PageSize));
        }

        [HttpGet]
        [Route(Common.Url.Project.Filter)]
        public async Task<IActionResult> Filter([FromBody]ProjectFilter projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Domain.Entities.Project, bool>> filter = item => true;
            if (projects != null)
            {
                if (!string.IsNullOrWhiteSpace(projects.Keyword))
                {
                    filter = filter.And(item => item.Title.Contains(projects.Keyword));
                }
                if (projects.CategoryId > 0)
                {
                    filter = filter.And(item => item.CategoryId == projects.CategoryId);
                }

                if (projects.CategoryId > 0)
                {
                    filter = filter.And(item => item.CategoryId == projects.CategoryId);
                }

                if (projects.SkillIds != null && projects.SkillIds.Any())
                {
                    filter = filter.And(item => item.ProjectSkills.Any(skill => projects.SkillIds.Contains(skill.SkillId)));
                }

                if (projects.Duration > 0)
                {
                    filter = filter.And(item => item.Duration <= projects.Duration);
                }

                if (projects.MinBudget > 0)
                {
                    filter = filter.And(item => item.MinBudget >= projects.MinBudget);
                }

                if (projects.MaxBudget > 0)
                {
                    filter = filter.And(item => item.MaxBudget <= projects.MaxBudget);
                }
            }
            return Ok(await _projectService.GetWithFilter(filter, projects.PageIndex, projects.PageSize));
        }

        [HttpGet]
        [Route(Common.Url.Project.GetProjectsByUserId)]
        public async Task<IActionResult> GetListByUserId([FromQuery] ProjectListDTO projects)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Domain.Entities.Project, bool>> filter = null;
            if (projects != null)
            {
                filter = item => item.CreatedBy == projects.UserId;
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
        public async Task<IActionResult> AddAsync(AddProjectDTO DTOs, CancellationToken token)
        {

            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            //await _projectService.Add(DTOs);
            //await _skillService.AddSkillForProject(DTOs.Skill, DTOs.Id);
            //return NoContent();

            var project = await _projectService.Add(DTOs);
            //nen tra ve 1 object
            await _skillService.AddSkillForProject(DTOs.Skill, project.Id);

            return Ok(new
            {
                success = true,
                message = "Bạn vừa tạo dự án thành công",
                data = project
            });
        }

        [HttpPut]
        [Route(Common.Url.Project.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateProjectDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var fetchedProject = await _projectRepository.GetByIdAsync(DTOs.Id);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy dự án phù hợp!" });
            }
            else
            {
                var project = await _projectService.Update(DTOs);

                await _skillService.AddSkillForProject(DTOs.Skill, project.Id);

                return Ok(new
                {
                    success = true,
                    message = "Bạn vừa cập nhật dự án thành công",
                    data = project
                });
            }


        }

        [HttpDelete]
        [Route(Common.Url.Project.Delete)]
        public async Task<IActionResult> DeleteAsync(int projectId, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var fetchedProject = await _projectRepository.GetByIdAsync(projectId);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy dự án phù hợp!" });
            }

            else
            {
                await _projectService.Delete(projectId);

                return Ok(new
                {
                    success = true,
                    message = "Bạn vừa xóa dự án thành công",
                    data = projectId
                });
            }
        }

        [HttpPut]
        [Route(Common.Url.Project.UpdateStatus)]
        public async Task<IActionResult> UpdateStatus(Application.DTOs.ProjectStatus DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var fetchedProject = await _projectRepository.GetByIdAsync(DTOs.Id);
            if (fetchedProject == null)
            {
                return NotFound(new { message = "Không tìm thấy dự án phù hợp!" });
            }
            else
            {
                var project = await _projectService.UpdateStatus(DTOs.Id, DTOs.StatusId);

                return Ok(new
                {
                    success = true,
                    message = "Bạn vừa thay đổi trạng thái dự án thành công",
                    data = project
                });
            }
        }

    }
}
