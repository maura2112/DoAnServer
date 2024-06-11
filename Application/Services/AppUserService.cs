using Application.DTOs;
using Application.IServices;
using AutoMapper;
using Domain.Common;
using Domain.IRepositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAddressRepository _addressRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly ISkillService _skillService;
        public AppUserService(IAppUserRepository repository, IMapper mapper, IAddressRepository addressRepository, IMediaFileRepository mediaFileRepository, ISkillService skillService)
        {
            _repository = repository;
            _mapper = mapper;
            _addressRepository = addressRepository;
            _mediaFileRepository = mediaFileRepository;
            _skillService = skillService;
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
                // quanlification

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
            }
            return userDTO;
        }
    }
}
