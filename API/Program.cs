
using Application.Mappings;
using FluentValidation;
using Infrastructure.Data;
using FluentValidation.AspNetCore;
using Application.DTOs;
using API.Middlewares;

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
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddTransient<RouteMiddleware>();
            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Services.AddDbContext<ApplicationDbContext>(opt => builder.Configuration.GetConnectionString("DefaultConnection"));
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<RouteMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
