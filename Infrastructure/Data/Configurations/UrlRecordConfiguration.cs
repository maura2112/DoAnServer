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
    public class UrlRecordConfiguration : IEntityTypeConfiguration<UrlRecord>
    {
        public void Configure(EntityTypeBuilder<UrlRecord> builder)
        {
            builder.ToTable("UrlRecords");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
