using Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
            _role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserService = (ICurrentUserService) context.HttpContext.RequestServices.GetService(typeof(ICurrentUserService));

            if (currentUserService == null || !currentUserService.HasRole(_role))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
