using Application.IServices;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        public SkillService(ISkillRepository skillRepository, IUserSkillRepository userSkillRepository)
        {
            _skillRepository = skillRepository;
            _userSkillRepository = userSkillRepository;
        }
        public async Task<List<Skill>> AddSkillForUser(List<string> skillNames , int uid)
        {
            var newSkills = new List<Skill>();
            var skills = new List<Skill>();
            foreach (var skillName in skillNames)
            {
                var skill  = await _skillRepository.GetByNameAsync(skillName.ToLower());
                if (skill != null)
                {
                    skills.Add(skill);
                }else
                {
                    skill = new Skill()
                    {
                        Id = 0,
                        CategoryId = (int) EnumCommon.Category.OtherType,
                        SkillName = skillName,
                        IsDeleted = false,
                    };
                    newSkills.Add(skill);
                }
            }
            await _skillRepository.SaveSkillsToOtherCategory(newSkills);
            skills.AddRange(newSkills);
            await _userSkillRepository.AddUserSkill(skills, uid);
            return skills;
        }
    }
}
