using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.EntitiesConfiguration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment").HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired().HasMaxLength(8000);
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.isDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(c => c.User)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.Post)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.PostId);
        }  
    }
}
