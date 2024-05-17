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
    public class BidCongiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("Bids");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.Proposal).IsRequired();
            builder.Property(x => x.Duration).IsRequired();
            builder.Property(x => x.Budget).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            //Relationship
            builder.HasOne(x => x.Project).WithMany(x => x.Bids).HasForeignKey(x => x.ProjectId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Bids).HasForeignKey(x => x.UserId);
        }
    }
}
