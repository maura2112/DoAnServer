using Application.DTOs.Favorite;
using Application.Extensions;
using Application.IServices;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext _context;
        public FavoriteService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<FavoriteDTO> CreateFavorite(FavoriteCreate create)
        //{
        //    var fa = new FavoriteProject()
        //    {
        //        AppUserId = (int)create.UserId,
        //        ProjectId = create.ProjectId,
        //        SavedDate = DateTime.Now,
        //    };
        //    await _context.FavoriteProjects.AddAsync(fa);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<Pagination<FavoriteDTO>> Gets(FavoriteSearch search)
        //{
        //    var query = from p in _context.Projects
        //                join f in _context.FavoriteProjects on p.Id equals f.ProjectId
        //                join u in _context.Users on f.AppUserId equals u.Id
        //                select new FavoriteDTO
        //                {
        //                    Id = f.Id,
        //                    ProjectId = f.ProjectId,
        //                    ProjectName = p.Title,
        //                    ProjectDescription = p.Description,
        //                    CreatedProjectTime = DateTimeHelper.ToVietnameseDateString(p.CreatedDate),
        //                    SavedTime = DateTimeHelper.ToVietnameseDateString(f.SavedDate)
        //                };
        //}
    }
}
