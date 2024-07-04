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
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");
            builder.HasKey(c => c.ConversationId);
            builder.Property(x => x.ConversationId).UseIdentityColumn();

            //Relationship
            builder.HasOne(x => x.User1Navigation).WithMany(x => x.User1Navigations).HasForeignKey(x => x.User1);
            builder.HasOne(x => x.User2Navigation).WithMany(x => x.User2Navigations).HasForeignKey(x => x.User2);
        }
    }
}
