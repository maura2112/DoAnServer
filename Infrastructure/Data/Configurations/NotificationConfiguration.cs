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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(c => c.NotificationId);
            builder.Property(n => n.NotificationId)
               .ValueGeneratedNever();
            builder.Property(t => t.Description)
                .HasMaxLength(1000);
               

            //Relationship
            builder.HasOne(x => x.RecieveNavigation).WithMany(x => x.RecieveNavigations).HasForeignKey(x => x.RecieveId);
            builder.HasOne(x => x.SendNavigation).WithMany(x => x.SendNavigations).HasForeignKey(x => x.SendId);
        }
    }
}
