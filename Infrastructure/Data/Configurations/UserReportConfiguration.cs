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
    public class UserReportConfiguration : IEntityTypeConfiguration<UserReport>
    {
        public void Configure(EntityTypeBuilder<UserReport> builder)
        {
            builder.ToTable("UserReports");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(t => t.ReportToUrl).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.IsApproved).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            // Relationship
            builder.HasOne(x => x.User).WithMany(x => x.UserReports).HasForeignKey(x => x.CreatedBy);
            builder.HasOne(x => x.ReportCategory).WithMany(x => x.UserReports).HasForeignKey(x => x.ReportCategoryId);

        }
    }
}
