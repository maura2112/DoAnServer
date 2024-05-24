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
    public class BidStageConfiguration : IEntityTypeConfiguration<BidStage>
    {
        public void Configure(EntityTypeBuilder<BidStage> builder)
        {
            builder.ToTable("BidStages");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.BidId).IsRequired();
            builder.Property(x => x.ProjectId).IsRequired();
            builder.Property(x => x.NumberStage).IsRequired();
            builder.Property(x => x.Decription).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();

            //relationShip
            builder.HasOne(x => x.Bid).WithMany(x => x.BidStages).HasForeignKey(x => x.BidId);
        }
    }
}
