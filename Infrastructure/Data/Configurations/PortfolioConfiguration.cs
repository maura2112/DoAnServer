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
    public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder.ToTable("Portfolios");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.MediaFileId).IsRequired();
   
            //Relationship
            builder.HasOne(x => x.AppUser).WithMany(x => x.Portfolios).HasForeignKey(x => x.UserId);
        }
    }
}
