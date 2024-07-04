
using Application.Mappings;
using FluentValidation;
using Infrastructure.Data;
using FluentValidation.AspNetCore;
using Application.DTOs;
using API.Middlewares;
using Application.IServices;
using Application.Services;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using Infrastructure.Services;
using API.Hubs;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            var mailsetting = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailsetting);
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title ="dotnetClaimAuthorization" , Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference =new OpenApiReference
                            {
                                Type  =ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddTransient<RouteMiddleware>();
            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Services.AddDbContext<ApplicationDbContext>(opt => builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddSignalR();

            builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowCredentials();

            }));
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<RouteMiddleware>();
            app.UseHttpsRedirection();
            app.MapHub<ChatHub>("/chat");

            app.UseAuthorization();

            app.UseCors();
            app.MapControllers();

            app.Run();
        }
    }
}
