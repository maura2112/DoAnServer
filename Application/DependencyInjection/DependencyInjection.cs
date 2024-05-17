
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Ardalis.GuardClauses;
using Infrastructure.Data;
using Domain.Entities;
using Application.Interfaces.IServices;
using Application.Services;
using Domain.IRepositories;
using Infrastructure.Repositories;
using Application.DTOs;
using FluentValidation.AspNetCore;
using FluentValidation;
using Application.Repositories;
using Application.IServices;

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

        //Url
        services.AddScoped<IUrlRepository, UrlRepository>();
        services.AddScoped<IUrlService, UrlService>();

        services.AddIdentityCore<AppUser>(opt =>
        {
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
