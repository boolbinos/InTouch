using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.EntitiesConfiguration
{
    class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
    {
        public void Configure(EntityTypeBuilder<PostLike> builder)
        {
            builder.ToTable("PostLike")
                .HasKey(pl => pl.Id);

            builder.HasOne(pl => pl.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId);

            builder.HasOne(pl => pl.User)
                .WithMany(u => u.PostsLike)
                .HasForeignKey(pl => pl.UserId);
        }
    }
}
