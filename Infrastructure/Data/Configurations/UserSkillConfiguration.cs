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
    public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
    {
        public void Configure(EntityTypeBuilder<UserSkill> builder)
        {
            builder.ToTable("UserSkills");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SkillId).IsRequired();
            //Relationship
            builder.HasOne(x => x.Skill).WithMany(x => x.UserSkills).HasForeignKey(x => x.SkillId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.UserSkills).HasForeignKey(x => x.UserId);
        }
    }
}
