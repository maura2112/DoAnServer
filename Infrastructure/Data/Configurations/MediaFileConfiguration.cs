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
    public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.ToTable("MediaFile");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.FolderId).IsRequired();
            builder.Property(x => x.Extension).IsRequired();
            builder.Property(x => x.Size).IsRequired();

            //Relationship
            builder.HasOne(x => x.MediaFolder).WithMany(x => x.MediaFiles).HasForeignKey(x => x.FolderId);
        }
    }
}
