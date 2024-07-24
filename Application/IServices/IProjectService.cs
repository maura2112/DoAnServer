using Application.DTOs;
using Application.DTOs.Favorite;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IProjectService
    {
        Task<Pagination<ProjectDTO>> Get(int pageIndex, int pageSize);

        Task<Pagination<ProjectDTO>> GetProjectDTOs(ProjectSearchDTO search);

        Task<ProjectDTO> UpdateProjectStatus(ProjectStatusUpdate update);

        Task<Pagination<ProjectBidDTO>> GetByStatus(ProjectStatusFilter search);

        Task<List<ProjectStatusDTO>> GetAllStatus();

        Task<Pagination<ProjectDTO>> GetWithFilter( ProjectSearchDTO dto, int pageIndex, int pageSize);
        Task<Pagination<ProjectDTO>> GetWithFilterRecruiter(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize);
        Task<Pagination<ProjectDTO>> GetWithFilterForRecruiter(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize);
        Task<Pagination<ProjectDTO>> GetByUserId(Expression<Func<Project, bool>> filter, int pageIndex, int pageSize);

        Task<ProjectDTO> GetDetailProjectById(int id);

        // not check delete
        Task<ProjectDTO> GetDetailProjectForId(int id);

        Task<ProjectDTO> Add(AddProjectDTO request);

        Task<ProjectDTO> Update(UpdateProjectDTO request);

        Task<ProjectDTO> Delete(int id);

        Task<ProjectDTO> UpdateStatus(int projectId, int statusId);

        //Favortite

        Task<Pagination<FavoriteDTO>> GetFavorites(FavoriteSearch search);

        Task<FavoriteDTO> GetFavoriteById(int uid, int pid);

        Task<int> DeleteFavorite(FavoriteCreate create);

        Task<bool> CreateFavorite(FavoriteCreate create);

        Task<bool?> IsFavorite(int userId, int projectId);


        //Task<int> CreateAsync(Project request);

        //Task<Pagination<ProjectDTO>> GetProjectByCategory(int id, int pageIndex, int pageSize);

    }
}
