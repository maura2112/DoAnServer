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
    public class HubConnectionConfiguration : IEntityTypeConfiguration<HubConnection>
    {
        public void Configure(EntityTypeBuilder<HubConnection> builder)
        {
            builder.ToTable("HubConnection");
        }
    }
}
