using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProjectSkillController : ApiControllerBase
    {
        private readonly IProjectSkillService _projectSkillService;
        private readonly ICurrentUserService _currentUserService;
        public ProjectSkillController(IProjectSkillService projectSkillService, ICurrentUserService currentUserService)
        {
            _projectSkillService = projectSkillService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [Route(Common.Url.ProjectSkill.GetAll)]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            return Ok(await _projectSkillService.Get(pageIndex, pageSize));
        }

        [HttpPost]
        [Route(Common.Url.ProjectSkill.Add)]
        public async Task<IActionResult> AddAsync(ProjectSkillDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            await _projectSkillService.Add(DTOs);
            return NoContent();
        }
    }
}
