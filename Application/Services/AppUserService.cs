using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAddressRepository _addressRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly ISkillService _skillService;
        private readonly PaginationService<UserDTO> _paginationService;
        private readonly IRatingService _ratingService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRateTransactionService _transactionService;
        public AppUserService(IAppUserRepository repository, IMapper mapper, IAddressRepository addressRepository, IMediaFileRepository mediaFileRepository, ISkillService skillService, UserManager<AppUser> userManager, PaginationService<UserDTO> paginationService, IRatingService ratingService, ICurrentUserService currentUserService, IRateTransactionService transactionService)
        {
            _repository = repository;
            _mapper = mapper;
            _addressRepository = addressRepository;
            _mediaFileRepository = mediaFileRepository;
            _skillService = skillService;
            _userManager = userManager;
            _paginationService = paginationService;
            _ratingService = ratingService;
            _currentUserService = currentUserService;
            _transactionService = transactionService;
        }

        public async Task<Pagination<UserDTO>> GetUsers(UserSearchDTO userSearch)
        {
            var users = new List<AppUser>();
            if (userSearch.role != null)
            {
                var userWithRole = await _userManager.GetUsersInRoleAsync(userSearch.role);
                users = userWithRole.ToList();
            }
            else
            {
                users = await _userManager.Users.ToListAsync();
            }
            if(userSearch.search != null)
            {
                users = users.Where(x => (x.Name != null && x.Name.ToLower().Contains(userSearch.search.ToLower())) ||
                         (x.Description != null && x.Description.ToLower().Contains(userSearch.search.ToLower())))
             .ToList();
            }
            if (userSearch.email != null)
            {
                users = users.Where(x => x.Email.ToLower().Contains(userSearch.email.ToLower())).ToList();
            }
            if (userSearch.phone != null)
            {
                users = users.Where(x => x.PhoneNumber != null).ToList();
                if(users.Count > 0)
                {
                    users = users.Where(x => x.PhoneNumber.ToLower().Contains(userSearch.phone.ToLower())).ToList();
                }
            }

            var userDTOS = users.Select(user =>
            {
                var userDTO = _mapper.Map<UserDTO>(user);
                if(userDTO.LockoutEnd > DateTime.Now && userDTO.LockoutEnabled != true) {
                    userDTO.IsLock =true;
                }
                var skillDTOs =  _skillService.GetForUser(user.Id);
                userDTO.skills = skillDTOs.Result.Select(x => x.SkillName).ToList();
                return userDTO;
            }).ToList();
            return await _paginationService.ToPagination(userDTOS, userSearch.PageIndex, userSearch.PageSize);
        }

        public async Task<UserDTO> GetUserDTOAsync(int uid)
        {
            var userDTO = new UserDTO();
            var user = await _repository.GetByIdAsync(uid); 
            if (user != null)
            {
                userDTO = _mapper.Map<UserDTO>(user);
                var  userAddress =  await _addressRepository.GetAddressByUserId(uid);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                if (userAddress != null)
                {
                    
                    userDTO.Address = _mapper.Map<AddressDTO>(userAddress);
                }
                if(user.Experience != null)
                {
                    var exs = System.Text.Json.JsonSerializer.Deserialize<List<Experience>>(user.Experience, options);
                    userDTO.Experiences= exs;
                }
                if (user.Education != null)
                {
                    var edu = System.Text.Json.JsonSerializer.Deserialize<List<Education>>(user.Education, options);
                    userDTO.Educations = edu;
                }
                if (user.Qualifications != null)
                {
                    var quali = System.Text.Json.JsonSerializer.Deserialize<List<Qualification>>(user.Qualifications, options);
                    userDTO.Qualifications = quali;
                }
                var medias = await _mediaFileRepository.GetByUserId(uid);
                if (medias.Any())
                {
                    userDTO.mediaFiles = _mapper.Map<List<MediaFileDTO>>(medias);
                }
                   var skillDTOs = await _skillService.GetForUser(uid);
                userDTO.skills = skillDTOs.Select(x=>x.SkillName).ToList();

                var ratings = await _ratingService.GetRatingsForUser(uid);
                if (ratings.Any())
                {
                    userDTO.ratings = ratings;
                }
                var userId = _currentUserService.UserId;
                if(userId != user.Id)
                {
                    var transaction = await _transactionService.GetRateTransactionByUsers(userId, user.Id);
                    if(transaction != null)
                    {
                        userDTO.IsRated = true;
                    }
                }
            }
            return userDTO;
        }

    }
}
