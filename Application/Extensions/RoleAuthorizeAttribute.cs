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
        private readonly string[] _roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserService = (ICurrentUserService)context.HttpContext.RequestServices.GetService(typeof(ICurrentUserService));

            if (currentUserService == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            bool isAuthorized = false;
            foreach (var role in _roles)
            {
                if (currentUserService.HasRole(role))
                {
                    isAuthorized = true;
                    break;
                }
            }

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
