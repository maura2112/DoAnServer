using Application.Common;
using Application.DTOs;
using Application.Extensions;
using Application.IServices;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Common;
using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
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
        private readonly IProjectRepository _projectRepository;
        private readonly IBidRepository _bidRepository;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;
        public AppUserService(IAppUserRepository repository, IMapper mapper, IAddressRepository addressRepository, IMediaFileRepository mediaFileRepository, ISkillService skillService, UserManager<AppUser> userManager, PaginationService<UserDTO> paginationService, IRatingService ratingService, ICurrentUserService currentUserService, IRateTransactionService transactionService, IProjectRepository projectRepository, IBidRepository bidRepository, ApplicationDbContext applicationDbContext, IEmailSender emailSender, ITokenService tokenService)
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
            _projectRepository = projectRepository;
            _bidRepository = bidRepository;
            _context = applicationDbContext;
            _emailSender = emailSender;
            _tokenService = tokenService;
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
            if (userSearch.search != null)
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
                if (users.Count > 0)
                {
                    users = users.Where(x => x.PhoneNumber.ToLower().Contains(userSearch.phone.ToLower())).ToList();
                }
            }

            var userDTOS = users.Select(user =>
            {
                var userDTO = ProcessUserDto(user);
                return userDTO.Result;
            }).ToList();
            return await _paginationService.ToPagination(userDTOS, userSearch.PageIndex, userSearch.PageSize);
        }

        public async Task<UserDTO> ProcessUserDto(AppUser user)
        {
            var userDTO = _mapper.Map<UserDTO>(user);
            if (userDTO.LockoutEnd > DateTime.Now && userDTO.LockoutEnabled != true)
            {
                userDTO.IsLock = true;
            }
            var filter = PredicateBuilder.True<Domain.Entities.Project>();
            filter = filter.And(item => item.CreatedBy == userDTO.Id);

            var filter2 = PredicateBuilder.True<Domain.Entities.Bid>();
            filter2 = filter2.And(item => item.UserId == userDTO.Id);

            userDTO.TotalProject = await _projectRepository.CountAsync(filter);
            userDTO.TotalBid = await _bidRepository.CountAsync(filter2);
            var skillDTOs = _skillService.GetForUser(userDTO.Id);
            userDTO.skills = skillDTOs.Result.Select(x => x.SkillName).ToList();
            return userDTO;
        }

        public async Task<UserDTO> GetUserDTOAsync(int uid)
        {
            var userDTO = new UserDTO();
            var user = await _repository.GetByIdAsync(uid);
            if (user != null)
            {
                userDTO = _mapper.Map<UserDTO>(user);
                var userAddress = await _addressRepository.GetAddressByUserId(uid);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                if (userAddress != null)
                {

                    userDTO.Address = _mapper.Map<AddressDTO>(userAddress);
                }
                if (user.Experience != null)
                {
                    var exs = System.Text.Json.JsonSerializer.Deserialize<List<Experience>>(user.Experience, options);
                    userDTO.Experiences = exs;
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
                userDTO.skills = skillDTOs.Select(x => x.SkillName).ToList();

                var ratings = await _ratingService.GetRatingsForUser(uid);
                if (ratings.Any())
                {
                    userDTO.ratings = ratings;
                }
                var userId = _currentUserService.UserId;
                if (userId != user.Id)
                {
                    var transaction = await _transactionService.GetRateTransactionByUsers(userId, user.Id);
                    if (transaction != null)
                    {
                        userDTO.IsRated = true;
                    }
                }
                var totalCompleteProject = await _context.RateTransactions.CountAsync(x => x.BidUserId == uid || x.ProjectUserId == uid);
                var totalRate = await _context.Ratings.CountAsync(x => x.RateToUserId == uid);
                decimal avgRate;
                if (totalRate != 0)
                {
                    float sumStars = await _context.Ratings.Where(x => x.RateToUserId == user.Id).SumAsync(x => x.Star);
                    avgRate = Math.Round((decimal)sumStars / totalRate, 1);
                }
                else
                {
                    avgRate = 0;
                }
                userDTO.AvgRate = avgRate;
                userDTO.TotalRate = totalRate;
                userDTO.TotalProject = totalCompleteProject;

            }
            return userDTO;
        }

        //public async Task<string> SendVerificationEmailAsync(string email)
        //{
        //    string uid = _currentUserService.UserId.ToString();
        //    var user = await _userManager.FindByIdAsync(uid);
        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //    // Create a unique token for the verification link
        //    var token = Guid.NewGuid().ToString();
        //    _tokenService.SaveToken(token, code);

        //    var callbackUrl = $"{baseUrl}/api/users/ConfirmEmail?token={token}";

        //    await _emailSender.SendEmailAsync(
        //        user.Email,
        //        "Xác nhận email",
        //        $"Vui lòng xác nhận email tài khoản của bạn bằng cách bấm vào link này: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //    return token;
        //}

        public async Task<AddressDTO> UpdateAddress(AddressDTO dto)
        {
            var userId = _currentUserService.UserId;
            dto.UserId = userId;
            dto.Country = "Việt Nam";
            var address = new Address()
            {
                UserId = userId,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                City = dto.City,
                State = dto.State,
                Street = dto.Street,
            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return dto;

        }

        public async Task<AppUser> FindByPhoneConfirmed(string phoneNumber)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber != null && x.PhoneNumber.Equals(phoneNumber) && x.PhoneNumberConfirmed == true);
            return user;
        }
    }
}
