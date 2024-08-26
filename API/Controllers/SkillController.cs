using Application.DTOs;
using Application.IServices;
using Domain.Entities;
using Domain.IRepositories;
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
        private readonly ISkillRepository _skillRepository;
        public SkillController(ISkillService skillService, ICurrentUserService currentUserService, ISkillRepository skillRepository)
        {
            _skillService = skillService;
            _skillRepository = skillRepository;
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
            var skilldb = await _skillService.GetSkillByNameAsyn(DTOs.SkillName);
            if (skilldb != null) {
                return BadRequest("Kỹ năng đã tồn tại");
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
            var skillFilters = await _skillService.GetWithFilter(filter, 1, int.MaxValue);
            skillFilters.Items.OrderBy(x => x.SkillName);
            return Ok(skillFilters);
        }
        [HttpGet]
        [Route(Common.Url.Skill.All)]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillService.GetAll();
            return Ok(skills.OrderBy(x=>x.SkillName));
        }

        [HttpGet]
        [Route(Common.Url.Skill.AllPublish)]
        public async Task<IActionResult> AllPublish([FromQuery] string skillname)
        {
            var skills = new List<Domain.Entities.Skill>();
            if(skillname == null)
            {
                skills = await _skillRepository.GetAll();
            }else
            {
                skills = await _skillRepository.GetsByNameAsync(skillname);
            }
            return Ok(skills.OrderBy(x=>x.SkillName));
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
        public async Task<IActionResult> AddSkillAsync([FromBody] SkillDTO DTOs)
        {
            if (!ModelState.IsValid)
            {
                
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            var uid =  _currentUserService.UserId;
            DTOs.CreatedBy = uid;
            await _skillService.Add(DTOs);
            return Ok();
        }
    }
}
