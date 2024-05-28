using Application.DTOs;
using Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SkillController : ApiControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly ICurrentUserService _currentUserService;
        public SkillController(ISkillService skillService, ICurrentUserService currentUserService)
        {
            _skillService = skillService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Route(Common.Url.Skill.Add)]
        public async Task<IActionResult> AddAsync(SkillDTO DTOs, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            await _skillService.Add(DTOs);
            return NoContent();
        }
    }
}
