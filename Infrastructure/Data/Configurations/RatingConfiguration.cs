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
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Rating");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.UserRatingToId).IsRequired();
            builder.Property(t => t.Comment)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(t => t.Star).IsRequired();
            
            //Relationship
            builder.HasOne(x => x.User).WithMany(x => x.UserRatingTo).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.UserRatingTo).WithMany(x => x.Projects).HasForeignKey(x => x.CategoryId);
        }
    }
}
