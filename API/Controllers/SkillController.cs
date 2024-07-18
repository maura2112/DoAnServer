using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static API.Common.Url;
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
            Expression<Func<Domain.Entities.Skill, bool>> filter = null;
            if (skills != null)
            {
                filter = item => item.CategoryId == skills.CategoryId;
            }
            return Ok(await _skillService.GetWithFilter(filter, 1, int.MaxValue));
        }
        [HttpGet]
        [Route(Common.Url.Skill.All)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _skillService.GetAll());
        }

        [HttpGet]
        [Route(Common.Url.Skill.Gets)]
        public async Task<IActionResult> Gets([FromQuery] SkillSearchDTO search)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var skills = await _skillService.Gets(search);
            return Ok(skills);
        }

        [HttpPut]
        [Route(Common.Url.Skill.Update)]
        public async Task<IActionResult> UpdateAsync([FromBody]SkillDTO DTOs)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            await _skillService.UpdateAsync(DTOs);
            return Ok(DTOs);
        }

        [HttpDelete]
        [Route(Common.Url.Skill.Delete)]
        public async Task<IActionResult> DeleteAsync([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            await _skillService.DeleteAsync(id);
            return Ok("Đã xóa kĩ năng  thành công");
        }

        [HttpPost]
        [Route(Common.Url.Skill.AddSkill)]
        public async Task<IActionResult> AddSkillAsync(SkillDTO DTOs)
        {
            if (!ModelState.IsValid)
            {
                
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var uid = _currentUserService.UserId;
            DTOs.CreatedBy = uid;
            await _skillService.Add(DTOs);
            return NoContent();
        }
    }
}
