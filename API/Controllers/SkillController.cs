using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
        [HttpGet]
        [Route(Common.Url.Skill.GetByCategoryId)]
        public async Task<IActionResult> Search([FromQuery] SkillListByCate skills)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            Expression<Func<Skill, bool>> filter = null;
            if (skills != null)
            {
                filter = item => item.CategoryId == skills.CategoryId;
            }
            return Ok(await _skillService.GetWithFilter(filter, skills.PageIndex, skills.PageSize));
        }

    }
}
