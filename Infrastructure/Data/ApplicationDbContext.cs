using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, Role ,int>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<MediaFolder> MediaFolders { get; set; }
        public virtual DbSet<MediaFile> MediaFiles { get; set; }
        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UrlRecord> UrlRecords { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectStatus> ProjectStatus { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<UserProject> UserProjects { get; set; }

    }
}
