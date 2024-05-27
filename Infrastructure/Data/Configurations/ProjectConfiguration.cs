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
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(t => t.Title)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x=>x.MinBudget).IsRequired();
            builder.Property(x=>x.MaxBudget).IsRequired();
            builder.Property(x=>x.Duration).IsRequired();
            builder.Property(x=>x.CreatedDate).IsRequired();
            //Relationship
            builder.HasOne(x => x.ProjectStatus).WithMany(x => x.Projects).HasForeignKey(x => x.StatusId);
            builder.HasOne(x => x.Category).WithMany(x => x.Projects).HasForeignKey(x => x.CategoryId);

            builder.HasMany(p => p.ProjectSkills)
            .WithOne(ps => ps.Project)
            .HasForeignKey(ps => ps.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
