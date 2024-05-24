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
    public class ProjectSkillConfiguration : IEntityTypeConfiguration<ProjectSkill>
    {
        public void Configure(EntityTypeBuilder<ProjectSkill> builder)
        {
            builder.ToTable("ProjectSkills");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.SkillId).IsRequired();
            //Relationship
            builder.HasOne(x => x.Skill).WithMany(x => x.ProjectSkills).HasForeignKey(x => x.SkillId);
            builder.HasOne(x => x.Project).WithMany(x => x.ProjectSkills).HasForeignKey(x => x.ProjectId);
        }
    }
}
