using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.EntitiesConfiguration
{
    class PostFileConfiguration:IEntityTypeConfiguration<PostFile>
    {
        public void Configure(EntityTypeBuilder<PostFile> builder)
        {
            builder.ToTable("PostFile")
                .HasKey(pf=>pf.Id);

            builder.HasOne(pf => pf.File)
                .WithOne(f => f.PostFile);

            builder.HasOne(pf => pf.Post)
                .WithMany(p => p.PostFiles)
                .HasForeignKey(pf=>pf.PostId);
        }
    }
}
