using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class LoginRespone
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string?  Avatar { get; set; }
        public string Role { get; set; }
    }
}
