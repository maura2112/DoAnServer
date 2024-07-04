
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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Application.Extensions;

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

        services.AddSingleton<IEmailSender, SendMailService>();

        //Cate
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();

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

        //Project
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        //Notification
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        //Bid
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddScoped<IBidService, BidService>();

        //Skill
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<ISkillRepository, SkillRepository>();

        //Skill
        services.AddScoped<IUserSkillRepository, UserSkillRepository>();

        //AppUser
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        //Address
        services.AddScoped<IAddressRepository, AddressRepository>();

        //ProjectSkil
        services.AddScoped<IProjectSkillRepository, ProjectSkillRepository>();
        services.AddScoped<IProjectSkillService, ProjectSkillService>();
        services.AddScoped<SignInManager<AppUser>>();
        services.AddScoped<UserManager<AppUser>>();
        services.AddScoped<RoleManager<Role>>();

        //Status
        //services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IStatusRepository, StatusRepository>();

        services.AddScoped<IProjectService, ProjectService>();


        //Paging 
        services.AddScoped<PaginationService<UserDTO>>();
        services.AddScoped<PaginationService<ReportDTO>>();
        services.AddScoped<PaginationService<ProjectDTO>>();

        //Report 
        services.AddScoped<IReportCategoryService, ReportCategoryService>();
        services.AddScoped<IReportCategoryRepository, ReportCategoryRepository>();
        services.AddScoped<IUserReportService, UserReportService>();
        services.AddScoped<IReportRepository, ReportRepository>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme)
        .AddCookie(IdentityConstants.ExternalScheme, options =>
        {
            options.Cookie.Name = IdentityConstants.ExternalScheme;
        })
        .AddGoogle(options =>
        {
            var gconfig = configuration.GetSection("Authentication:Google");
            options.ClientId = gconfig["ClientId"];
            options.ClientSecret = gconfig["ClientSecret"];
            options.CallbackPath = new PathString("/dang-nhap-tu-google"); // Ensure this matches the Google Cloud Console settings
        });


        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.User.AllowedUserNameCharacters = null;
            opt.Lockout.AllowedForNewUsers = true; // Default true
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
            opt.Lockout.MaxFailedAccessAttempts = 5; // Default 5
        })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddSingleton(TimeProvider.System);
        return services;
    }
}
