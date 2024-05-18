
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Ardalis.GuardClauses;
using Infrastructure.Data;
using Domain.Entities;
using Application.Services;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Application.DTOs;
using FluentValidation.AspNetCore;
using FluentValidation;
using Application.Repositories;
using Application.IServices;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(typeof(ProductDTOValidator));
        //Product
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        //Media
        services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        services.AddScoped<IMediaService, MediaService>();

        services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
        //JWT token
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        //Url
        services.AddScoped<IUrlRepository, UrlRepository>();
        services.AddScoped<IUrlService, UrlService>();

        //Project
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectService, ProjectService>();

        //Bid
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddScoped<IBidService, BidService>();

        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.User.AllowedUserNameCharacters = null;
            opt.Lockout.AllowedForNewUsers = true; // Default true
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
            opt.Lockout.MaxFailedAccessAttempts = 3; // Default 5
        })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddSingleton(TimeProvider.System);
        return services;
    }
}
