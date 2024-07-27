using Application.DTOs;
using Domain.Common;
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
        Task<string> SendVerificationEmailAsync(string link);

        Task<AddressDTO> UpdateAddress(AddressDTO dto);

        //Task<UserDTO> AddBidAsync(int amount);
    }
}
