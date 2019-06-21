using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.EntitiesConfiguration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post").HasKey(p => p.Id);
            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Content).IsRequired().HasMaxLength(5000);
            builder.Property(p => p.PostType).HasDefaultValue(PostType.InTouch);
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.VideoUrl);
            builder.Property(p => p.Priority).HasDefaultValue(PostPriority.Normal);
            builder.Property(p => p.IsPublic);
            builder.Property(p => p.IsDeleted).IsRequired();
            builder.Property(p => p.ModifiedByUserId);
            builder.Property(p => p.ModifiedDate);
            builder.HasOne(p => p.ModifiedByUser);
            builder.Property(p => p.DurationInSeconds).HasDefaultValue(10);
            builder.Property(p => p.StartDateTime);
            builder.Property(p => p.EndDateTime);
            builder.HasOne(p => p.User).WithMany(t => t.Posts).HasForeignKey(p => p.UserId);
            builder.HasMany(c => c.Comments).WithOne(c => c.Post).HasPrincipalKey(c => c.Id);
        }
    }
}
