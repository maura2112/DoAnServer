using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.ToTable("Bookmarks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SavedDate).IsRequired();

            //Relationship
            builder.HasOne(x => x.AppUser).WithMany(x => x.Bookmarks).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Project).WithMany(x => x.Bookmarks).HasForeignKey(x => x.ProjectId);

        }
    }
}
