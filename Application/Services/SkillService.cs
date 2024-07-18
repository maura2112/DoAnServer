using Application.Common;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Azure.Core;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        private readonly ISkillRepository _skillRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUrlRepository _urlRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectSkillRepository _projectSkillRepository;
        private readonly IMapper _mapper;
        public SkillService(ISkillRepository skillRepository, IUserSkillRepository userSkillRepository, IMapper mapper, ICategoryRepository categoryRepository, IUrlRepository urlRepository, IProjectRepository projectRepository, IProjectSkillRepository projectSkillRepository, ApplicationDbContext context)
        {
            _skillRepository = skillRepository;
            _userSkillRepository = userSkillRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _urlRepository = urlRepository;
            _projectRepository = projectRepository;
            _projectSkillRepository = projectSkillRepository;
            _context = context;
        }

        public async Task<int> Add(SkillDTO request)
        {
            var skill = _mapper.Map<Skill>(request);
            skill.CategoryId = request.CategoryId;
            skill.SkillName = request.SkillName;
            skill.IsDeleted = request.IsDeleted;
            if(request.CreatedBy != null)
            {
                skill.CreatedBy = request.CreatedBy;
            }
            skill.CreatedDate = DateTime.Now;
            //media file
            await _skillRepository.AddAsync(skill);
            return skill.Id;
        }

        public async Task<int> UpdateAsync(SkillDTO request)
        {
            var skill = await _skillRepository.GetByIdAsync(request.Id);
            if(skill == null)
            {
                return 0;
            }
            skill.CategoryId = request.CategoryId;
            skill.SkillName = request.SkillName;
            skill.IsDeleted = request.IsDeleted;
            skill.UpdatedDate = DateTime.Now;
            //media file
            _skillRepository.Update(skill);
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

        public async Task<List<SkillDTO>> GetForUser(int uid)
        {
            var skills = await _skillRepository.GetSkillByUser(uid);
             var skillDTOs = new List<SkillDTO>();
            if (skills == null)
            {
                return skillDTOs;
            }
            skillDTOs =  _mapper.Map<List<SkillDTO>>(skills);
            return skillDTOs;
        }

        public async Task<Pagination<SkillDTO>> Gets(SkillSearchDTO search)
        {
            var query = from s in _context.Skills
                        join c in _context.Categories on s.CategoryId equals c.Id
                        join u in _context.Users on s.CreatedBy equals u.Id
                        where s.IsDeleted != true 
                        select new SkillDTO
                        {
                            Id = s.Id,
                            SkillName = s.SkillName,
                            CreatedBy = (u != null) ? u.Id : 0,
                            CreatedByName = (u != null)?u.Name:"",
                            CreatedTime = (s.CreatedDate != null) ? DateTimeHelper.ToVietnameseDateString(s.CreatedDate): "---",
                            UpdatedTime = (s.UpdatedDate != null) ? DateTimeHelper.ToVietnameseDateString(s.UpdatedDate) : "---",
                            CategoryId = s.CategoryId,
                            CategoryName = c.CategoryName,
                        };

            var filter = PredicateBuilder.True<SkillDTO>();
            if(search.SkillName != null)
            {
                filter = filter.And(item => item.SkillName.ToLower().Contains(search.SkillName.ToLower()));
            }
            if (search.CategoryId != null)
            {
                filter = filter.And(item => item.CategoryId == search.CategoryId);
            }
            query = query.Where(filter);
            var totalItem = await query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToListAsync();
            var result = new Pagination<SkillDTO>()
            {
                PageSize = search.PageSize,
                PageIndex = search.PageIndex,
                TotalItemsCount = query.Count(),
                Items = totalItem,
            };
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);
            if(skill == null)
            {
                return false;
            }
            skill.IsDeleted = true;
            _skillRepository.Update(skill);
            return true;
        }
    }
}
