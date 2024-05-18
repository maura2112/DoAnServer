using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUrlRepository _urlRepository;
        public ProjectService(IMapper mapper, IProjectRepository projectRepository, IUrlRepository urlRepository)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _urlRepository = urlRepository;
        }

        public async Task<int> Add(ProjectDTO request)
        {
            var project = _mapper.Map<Project>(request);
            project.CategoryId = request.CategoryId;
            project.MinBudget = request.MinBudget;
            project.MaxBudget = request.MaxBudget;
            project.Duration = request.Duration;
            // createdBy
            project.CreatedDate = DateTime.Now;
            project.UpdatedDate = DateTime.Now;
            project.StatusId = 1;
            //media file
            await _projectRepository.AddAsync(project);
            var urlRecord = project.CreateUrlRecordAsync("du-an", project.Title);
            await _urlRepository.AddAsync(urlRecord);
            return project.Id;
        }

        public async Task<Pagination<ProjectDTO>> Get(int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.ToPagination(pageIndex, pageSize);
            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            return projectDTOs;
        }

        public async Task<ProjectDTO> GetDetailProjectById(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            return projectDTO;
        }

        

        public async Task<Pagination<ProjectDTO>> GetWithFilter(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize)
        {
            var projects = await _projectRepository.GetAsync(filter, pageIndex, pageSize);

            var projectDTOs = _mapper.Map<Pagination<ProjectDTO>>(projects);
            return projectDTOs;
        }
    }
}
