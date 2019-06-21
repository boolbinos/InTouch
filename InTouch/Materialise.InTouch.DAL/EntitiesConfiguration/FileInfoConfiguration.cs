using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.EntitiesConfiguration
{
    public class FileInfoConfiguration:IEntityTypeConfiguration<FileInfo>
    {
        public void Configure(EntityTypeBuilder<FileInfo> builder)
        {
            builder.ToTable("File").HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.ContentType).IsRequired().HasMaxLength(500);
            builder.Property(p => p.SizeKB).IsRequired();
            builder.Property(p => p.Name).IsRequired();
                
        }
    }
}
