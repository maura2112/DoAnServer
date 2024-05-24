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
    public class BidStageConfiguration : IEntityTypeConfiguration<BidStage>
    {
        public void Configure(EntityTypeBuilder<BidStage> builder)
        {
            builder.ToTable("BidStages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.TotalOfStage).IsRequired();
            builder.Property(x => x.IsAccepted).IsRequired();
            builder.Property(x => x.BidId).IsRequired();
        }  
    }
}
