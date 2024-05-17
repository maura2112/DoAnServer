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
    public class ProjectStatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
    {
        public void Configure(EntityTypeBuilder<ProjectStatus> builder)
        {
            builder.ToTable("ProjectStatus");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(t => t.StatusName)
                .HasMaxLength(200)
                .IsRequired();
           
        }
    }
}
