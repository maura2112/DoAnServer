using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IAppUserService
    {
        Task<UserDTO> GetUserDTOAsync(int uid);
        Task<Pagination<UserDTO>> GetUsers(UserSearchDTO userSearch);
        //Task<string> SendVerificationEmailAsync(string email);

        Task<AddressDTO> UpdateAddress(AddressDTO dto);

        Task<AppUser> FindByPhoneConfirmed(string phoneNumber);

        Task<List<UserDTO>> GetUsersLocked();
        //Task<UserDTO> AddBidAsync(int amount);
    }
}
