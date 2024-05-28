using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProjectSkillService : IProjectSkillService
    {
        private readonly IMapper _mapper;
        private readonly IProjectSkillRepository _projectSkillRepository;
        private readonly IUrlRepository _urlRepository;
        public ProjectSkillService(IMapper mapper, IProjectSkillRepository projectSkillRepository, IUrlRepository urlRepository)
        {
            _mapper = mapper;
            _projectSkillRepository = projectSkillRepository;
            _urlRepository = urlRepository;
        }

        public async Task<int> Add(ProjectSkillDTO request)
        {
            var projectSkill = _mapper.Map<ProjectSkill>(request);
            projectSkill.SkillId = request.SkillId;
            projectSkill.ProjectId = request.ProjectId;
            
            await _projectSkillRepository.AddAsync(projectSkill);
            //var urlRecord = projectSkill.CreateUrlRecordAsync("ki-nang-du-an", projectSkill.Skill.GetEntityName());
            //await _urlRepository.AddAsync(urlRecord);
            return projectSkill.Id;
        }

        public async Task<Pagination<ProjectSkillDTO>> Get(int pageIndex, int pageSize)
        {
            var projectSkills = await _projectSkillRepository.ToPagination(pageIndex, pageSize);
            var projectSkillDTOs = _mapper.Map<Pagination<ProjectSkillDTO>>(projectSkills);
            return projectSkillDTOs;
        }

        public async Task<Pagination<ProjectSkillDTO>> GetWithFilter(Expression<Func<ProjectSkill, bool>> filter, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
