using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Azure.Core;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUrlRepository _urlRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProjectSkillRepository _projectSkillRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly PaginationService<ProjectDTO> _paginationService;


        public ProjectService(IMapper mapper, IProjectRepository projectRepository, IUrlRepository urlRepository, IAppUserRepository appUserRepository, ICategoryRepository categoryRepository, IProjectSkillRepository projectSkillRepository, ICurrentUserService currentUserService, IAddressRepository addressRepository, IStatusRepository statusRepository, PaginationService<ProjectDTO> paginationService)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _urlRepository = urlRepository;
            _appUserRepository = appUserRepository;
            _categoryRepository = categoryRepository;
            _projectSkillRepository = projectSkillRepository;
            _currentUserService = currentUserService;
            _addressRepository = addressRepository;
            _statusRepository = statusRepository;
            _paginationService = paginationService;
        }

        public async Task<ProjectDTO> Add(AddProjectDTO request)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return null;
            }
            var project = _mapper.Map<Project>(request);
            var existedCategory = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (existedCategory == null)
            {
                return null; 
            }
            project.CategoryId = request.CategoryId;
            project.MinBudget = request.MinBudget;
            project.MaxBudget = request.MaxBudget;
            project.Duration = request.Duration;
            // createdBy
            project.CreatedBy = userId;
            project.CreatedDate = DateTime.Now;
            project.StatusId = 1;
            project.IsDeleted = false;
            project.Description = request.Description;


            //

            

            //media file

            try
            {
                await _projectRepository.AddAsync(project);
            }
            catch (Exception ex)
            {
                throw new Exception("Tạo dự án mới thất bại", ex);
            }
            
            //var urlRecord = project.CreateUrlRecordAsync("tao-du-an", project.Title);
            //await _urlRepository.AddAsync(urlRecord);

            var projectDto = _mapper.Map<ProjectDTO>(project);
            //smthing ưởng here
            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDto.AppUser = _mapper.Map<AppUserDTO>(user);

            //var address = await _addressRepository.GetAddressByUserId(userId);
            //projectDto.AppUser.Address = _mapper.Map<AddressDTO>(address);

            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDto.Category = _mapper.Map<CategoryDTO>(category);

            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDto.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            foreach (var skill in listSkills)
            {
                projectDto.Skill.Add(skill.SkillName);
            }

            projectDto.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDto.CreatedDate);
            projectDto.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.CreatedDate);
            projectDto.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.UpdatedDate);

            return projectDto;
        }


        public async Task<ProjectDTO> Delete(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new Exception($"Project with ID {id} not found.");
            }
            project.IsDeleted = true;
             _projectRepository.Update(project);
            var projectDto = _mapper.Map<ProjectDTO>(project);
            return projectDto;
        }

        public async Task<Pagination<ProjectDTO>> Get(int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.ToPagination(pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            var updatedItems = new List<ProjectDTO>();

            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);

                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = user != null ? _mapper.Map<AppUserDTO>(user) : null;

                var address = user != null ? await _addressRepository.GetAddressByUserId((int)x.CreatedBy) : null;
                if (model.AppUser != null && address != null)
                {
                    model.AppUser.Address = _mapper.Map<AddressDTO>(address);
                }

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = category != null ? _mapper.Map<CategoryDTO>(category) : null;

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = status != null ? _mapper.Map<ProjectStatusDTO>(status) : null;

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                if (listSkills != null)
                {
                    foreach (var skill in listSkills)
                    {
                        model.Skill.Add(skill.SkillName);
                    }
                }

                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }

            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<ProjectDTO> GetDetailProjectById(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if(project == null)
            {
                return null;
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);

            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDTO.AppUser = _mapper.Map<AppUserDTO>(user);

            var address = await _addressRepository.GetAddressByUserId((int)project.CreatedBy);
            projectDTO.AppUser.Address = _mapper.Map<AddressDTO>(address);

            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDTO.Category = _mapper.Map<CategoryDTO>(category);

            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDTO.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            projectDTO.Skill = listSkills.Select(x=>x.SkillName).ToList();
            projectDTO.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDTO.CreatedDate);
            projectDTO.AverageBudget = await _projectRepository.GetAverageBudget(projectDTO.Id);
            projectDTO.TotalBids = await _projectRepository.GetTotalBids(projectDTO.Id);
            projectDTO.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.CreatedDate);
            projectDTO.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.UpdatedDate);
            return projectDTO;
        }

        public async Task<Pagination<ProjectDTO>> GetProjectDTOs(ProjectSearchDTO search)
        {
            var projects = await _projectRepository.GetAll();
            var res = projects.AsQueryable();
            var projectDTOs =   projects.Select(project => ProcessProjectAsync(project).Result).ToList();

            var projectDTOList = projectDTOs.AsQueryable();
            if (search.Keyword != null)
            {
                projectDTOList = projectDTOList.Where(x => x.Title.ToLower().Contains(search.Keyword.ToLower()) || x.Description.ToLower().Contains(search.Keyword.ToLower()));
            }
            if (search.Skill != null)
            {
                foreach (var skill in search.Skill)
                {
                    projectDTOList = projectDTOList.Where(x=>x.Skill.Contains(skill));
                }
            }
            if(search.StatusId != null)
            {
                projectDTOList = projectDTOList.Where(x => x.StatusId == search.StatusId);
            }
            if(search.MinBudget !=null)
            {
                projectDTOList = projectDTOList.Where(x => x.MinBudget >=  search.MinBudget);
            }
            if (search.MaxBudget != null)
            {
                projectDTOList = projectDTOList.Where(x => x.MaxBudget <= search.MaxBudget);
            }
            if (search.CategoryId != null)
            {
                projectDTOList = projectDTOList.Where(x => x.CategoryId == search.CategoryId);
            }
            if (search.CreatedFrom != null)
            {
                projectDTOList = projectDTOList.Where(x => x.CreatedDate >= search.CreatedFrom);
            }
            if (search.CreatedTo != null)
            {
                projectDTOList = projectDTOList.Where(x => x.CreatedDate <= search.CreatedTo);
            }
            return await _paginationService.ToPagination(projectDTOList.ToList(), search.PageIndex, search.PageSize) ;
        }

        public async Task<ProjectDTO> ProcessProjectAsync(Project project)
        {
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            //category
            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDTO.Category = _mapper.Map<CategoryDTO>(category);
            //user
            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDTO.AppUser = _mapper.Map<AppUserDTO>(user);
            //address
            var address = user != null ? await _addressRepository.GetAddressByUserId((int)projectDTO.CreatedBy) : null;
            if (projectDTO.AppUser != null && address != null)
            {
                projectDTO.AppUser.Address = _mapper.Map<AddressDTO>(address);
            }
            //status
            var status = await _statusRepository.GetByIdAsync(projectDTO.StatusId);
            projectDTO.ProjectStatus = status != null ? _mapper.Map<ProjectStatusDTO>(status) : null;
            //Skill
            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(projectDTO.Id);
            projectDTO.Skill = listSkills.Select(x => x.SkillName).ToList();

            projectDTO.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDTO.CreatedDate);
            projectDTO.AverageBudget = await _projectRepository.GetAverageBudget(projectDTO.Id);
            projectDTO.TotalBids = await _projectRepository.GetTotalBids(projectDTO.Id);
            projectDTO.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.CreatedDate);
            projectDTO.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDTO.UpdatedDate);
            return projectDTO;
        }

        public async Task<List<ProjectStatusDTO>> GetAllStatus()
        {
            var statuses = await _statusRepository.GetAll();
            var statuseDTO = _mapper.Map<List<ProjectStatusDTO>>(statuses);
            return statuseDTO;
        }

        public async Task<ProjectDTO> UpdateProjectStatus(int statusId, int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            project.StatusId = statusId;
            project.UpdatedDate = DateTime.Now;
            _projectRepository.Update(project);
            var DTO = _mapper.Map<ProjectDTO>(project);
            var status = await _statusRepository.GetByIdAsync(DTO.StatusId);
            DTO.ProjectStatus = status != null ? _mapper.Map<ProjectStatusDTO>(status) : null;
            return DTO;
        }

        public async Task<Pagination<ProjectDTO>> GetWithFilter(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.GetAsync(filter, pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            var updatedItems = new List<ProjectDTO>();

            foreach (var x in projectDTOs.Items)
            {
                var model = _mapper.Map<ProjectDTO>(x);

                var user = await _appUserRepository.GetByIdAsync(x.CreatedBy);
                model.AppUser = _mapper.Map<AppUserDTO>(user);
                var address = await _addressRepository.GetAddressByUserId((int)x.CreatedBy);
                model.AppUser.Address = _mapper.Map<AddressDTO>(address);

                var category = await _categoryRepository.GetByIdAsync(x.CategoryId);
                model.Category = _mapper.Map<CategoryDTO>(category);

                var status = await _statusRepository.GetByIdAsync(x.StatusId);
                model.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

                var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(x.Id);
                foreach (var skill in listSkills)
                {
                    model.Skill.Add(skill.SkillName);
                }
                model.TimeAgo = TimeAgoHelper.CalculateTimeAgo(model.CreatedDate);
                model.AverageBudget = await _projectRepository.GetAverageBudget(model.Id);
                model.TotalBids = await _projectRepository.GetTotalBids(model.Id);
                model.CreatedDateString = DateTimeHelper.ToVietnameseDateString(model.CreatedDate);
                model.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(model.UpdatedDate);
                updatedItems.Add(model);
            }

            projectDTOs.Items = updatedItems;
            return projectDTOs;
        }

        public async Task<ProjectDTO> Update(UpdateProjectDTO request)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);

            // Update the project's properties
            project.Title = request.Title;
            project.CategoryId = request.CategoryId;
            project.MinBudget = request.MinBudget;
            project.MaxBudget = request.MaxBudget;
            project.Duration = request.Duration;
            project.UpdatedDate = DateTime.Now; // update the updated date
            //project.CreatedBy = request.CreatedBy;
            project.Description = request.Description;
            //mediafile

            // Update the project in the repository
            _projectRepository.Update(project);

            // Handle URL record update
            //var urlRecord = project.CreateUrlRecordAsync("chinh-sua-du-an", project.Title);
            //await _urlRepository.AddAsync(urlRecord); // assuming there's a method for updating URLs

            // Map the updated project back to a DTO
            var projectDto = _mapper.Map<ProjectDTO>(project);

            // Retrieve and map the user who created the project
            var user = await _appUserRepository.GetByIdAsync(project.CreatedBy);
            projectDto.AppUser = _mapper.Map<AppUserDTO>(user);

            var address = await _addressRepository.GetAddressByUserId((int)project.CreatedBy);
            projectDto.AppUser.Address = _mapper.Map<AddressDTO>(address);

            // Retrieve and map the category of the project
            var category = await _categoryRepository.GetByIdAsync(project.CategoryId);
            projectDto.Category = _mapper.Map<CategoryDTO>(category);

            var status = await _statusRepository.GetByIdAsync(project.StatusId);
            projectDto.ProjectStatus = _mapper.Map<ProjectStatusDTO>(status);

            await _projectSkillRepository.DeleteProjectSkill(project.Id);

            // Retrieve and map the skills associated with the project
            var listSkills = await _projectSkillRepository.GetListProjectSkillByProjectId(project.Id);
            foreach (var skill in listSkills)
            {
                projectDto.Skill.Add(skill.SkillName);
            }
            projectDto.TimeAgo = TimeAgoHelper.CalculateTimeAgo(projectDto.CreatedDate);
            projectDto.AverageBudget = await _projectRepository.GetAverageBudget(projectDto.Id);
            projectDto.TotalBids = await _projectRepository.GetTotalBids(projectDto.Id);
            projectDto.CreatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.CreatedDate);
            projectDto.UpdatedDateString = DateTimeHelper.ToVietnameseDateString(projectDto.UpdatedDate);



            return projectDto;
        }

        public async Task<ProjectDTO> UpdateStatus(int projectId, int statusId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new Exception("Project not found");
            }
            project.StatusId = statusId;



            var projectDto = _mapper.Map<ProjectDTO>(project);
            _projectRepository.Update(project);

            return projectDto;
        }
    }
}
