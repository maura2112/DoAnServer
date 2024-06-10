using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Azure.Core;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUrlRepository _urlRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectSkillRepository _projectSkillRepository;
        private readonly IMapper _mapper;
        public SkillService(ISkillRepository skillRepository, IUserSkillRepository userSkillRepository, IMapper mapper, ICategoryRepository categoryRepository, IUrlRepository urlRepository, IProjectRepository projectRepository, IProjectSkillRepository projectSkillRepository)
        {
            _skillRepository = skillRepository;
            _userSkillRepository = userSkillRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _urlRepository = urlRepository;
            _projectRepository = projectRepository;
            _projectSkillRepository = projectSkillRepository;
        }

        public async Task<int> Add(SkillDTO request)
        {
            var skill = _mapper.Map<Skill>(request);
            skill.CategoryId = request.CategoryId;
            skill.SkillName = request.SkillName;
            skill.IsDeleted = request.IsDeleted;
            //media file
            await _skillRepository.AddAsync(skill);
            var urlRecord = skill.CreateUrlRecordAsync("ky-nang-nguoi-dung", skill.SkillName);
            await _urlRepository.AddAsync(urlRecord);
            return skill.Id;
        }

        public async Task<List<Skill>> AddSkillForProject(List<string> skillNames, int pId)
        {
            var newSkills = new List<Skill>();
            var skills = new List<Skill>();
            var categoryOrtherId = await _categoryRepository.GetIdCatetegoryOther();
            foreach (var skillName in skillNames)
            {
                var skill = await _skillRepository.GetByNameAsync(skillName.ToLower());
                if (skill != null)
                {
                    skills.Add(skill);
                }
                else
                {
                    skill = new Skill()
                    {
                        Id = 0,
                        CategoryId = categoryOrtherId,
                        SkillName = skillName,
                        IsDeleted = false,
                    };
                    newSkills.Add(skill);
                }
            }
            await _skillRepository.SaveSkillsToOtherCategory(newSkills);
            skills.AddRange(newSkills);
            await _projectSkillRepository.AddProjectSkill(skills, pId);
            return skills;
        }

        public async Task<List<Skill>> AddSkillForUser(List<string> skillNames , int uid)
        {
            var newSkills = new List<Skill>();
            var skills = new List<Skill>();
            var categoryOrtherId = await _categoryRepository.GetIdCatetegoryOther();
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
                        CategoryId = categoryOrtherId,
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

        public async Task<List<Skill>> UpdateSkillForUser(List<string> skillNames, int uid)
        {
            await _userSkillRepository.RemoveUserSkill(uid);
            var skills = await AddSkillForUser(skillNames, uid);
            return skills;
        }

        public Task<Pagination<SkillDTO>> Get(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SkillDTO>> GetAll() {
            var skills= await _skillRepository.GetAll();
            var skillDTos =  _mapper.Map<List<SkillDTO>>(skills);
            foreach (var skillDto in skillDTos)
            {
                var cate = await _categoryRepository.GetByIdAsync(skillDto.CategoryId);
                skillDto.CategoryName = cate.CategoryName;
            }
            return skillDTos;
        }
        public async Task<Pagination<SkillDTO>> GetWithFilter(Expression<Func<Skill, bool>> filter, int pageIndex, int pageSize)
        {
            var skills = await _skillRepository.GetAsync(filter, pageIndex, pageSize);
            var skillDTOs = _mapper.Map<Pagination<SkillDTO>>(skills);
            return skillDTOs;
        }

        
    }
}
