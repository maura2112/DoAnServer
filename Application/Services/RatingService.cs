using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context; 
        private readonly IRatingRepository _repository;
        private readonly IMapper _mapper;
        public RatingService(IRatingRepository repository, IMapper mapper, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }
        public async Task<RatingDTO> CreateRating(RatingDTO dto)
        {
            var rating = _mapper.Map<Rating>(dto);
             await _repository.AddAsync(rating);
            return dto;
        }

        public async Task<List<RatingDTO>> GetRatingsForUser(int uid)
        {
            //var rating = await _repository.Ratings(uid);
            var query = from r in _context.Ratings
                        join u in _context.Users on r.UserId equals u.Id into userGroup
                        from user in userGroup.DefaultIfEmpty()
                        join t in _context.RateTransactions on r.RateTransactionId equals t.Id into transactionGroup
                        from transaction in transactionGroup.DefaultIfEmpty()
                        join p in _context.Projects on transaction.ProjectId equals p.Id into projectGroup
                        from project in projectGroup.DefaultIfEmpty()
                        where r.RateToUserId == uid
                        select new RatingDTO
                        {
                            Id = r.Id,
                            Comment = r.Comment,
                            UserId = r.UserId,
                            Star = r.Star,
                            UserRate = user.Name,
                            ProjectName = project.Title,
                            ProjectId = project.Id,
                        };

            var resultList = await query.ToListAsync();
            return resultList;
        }
    }
}
