using Application.DTOs;
using Domain.Common;
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
    }
}
