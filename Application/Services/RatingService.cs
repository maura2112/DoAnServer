using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RatingService : IRatingService
    {

        private readonly IRatingRepository _repository;
        private readonly IMapper _mapper;
        public RatingService(IRatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<RatingDTO> CreateRating(RatingDTO dto)
        {
            var rating = _mapper.Map<Rating>(dto);
             await _repository.AddAsync(rating);
            return dto;
        }
    }
}
