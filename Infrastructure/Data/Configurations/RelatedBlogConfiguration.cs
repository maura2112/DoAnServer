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
    public class RelatedBlogConfiguration : IEntityTypeConfiguration<RelatedBlog>
    {
        public void Configure(EntityTypeBuilder<RelatedBlog> builder)
        {
            builder.ToTable("RelatedBlogs");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.CreatedDate).IsRequired();

            //Relationship
            builder.HasOne(x => x.Blog).WithMany(x => x.RelatedBlogs).HasForeignKey(x => x.BlogId);
        }
    }
}
