using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.ToTable("UserProjects");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            //relation
            builder.HasOne(x => x.Project).WithMany(x => x.UserProjects).HasForeignKey(x => x.ProjectId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.UserProjects).HasForeignKey(x => x.UserId);
        }
    }
}
