
using Application.IServices;
using Microsoft.AspNetCore.Http;

namespace API.Middlewares
{
    public class RouteMiddleware : IMiddleware
    {

        private readonly IUrlService _urlService;
        public RouteMiddleware(IUrlService urlService)
        {
            _urlService = urlService;
        }
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            if (httpContext.Request.Method == "GET")
            {
                var secFetchDest = httpContext.Request.Headers["Sec-Fetch-Dest"];
                if (secFetchDest == "document")
                {
                    var path = httpContext.Request.Path.ToString().ToLower().TrimStart('/').TrimEnd('/');
                    
                }
            }

            await next(httpContext);
        }
    }
}
