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
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(t => t.Street)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(t => t.City)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(t => t.State)
                .HasMaxLength(1000)
                .IsRequired();
            builder.Property(t => t.PostalCode)
                .HasMaxLength(1000)
                .IsRequired();
            
            builder.Property(t => t.Country)
                .HasMaxLength(1000)
                .IsRequired();

            //Relationship
            builder.HasOne(x => x.AppUser).WithOne(x => x.Address);
        }
    }
}
