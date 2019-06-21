using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User").HasKey(p => p.Id);
            builder.Property(p => p.FirstName).HasMaxLength(50);
            builder.Property(p => p.LastName).HasMaxLength(50);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(50);
            builder.Property(p => p.IsDeleted).IsRequired();
            builder.HasIndex(p => p.Email).IsUnique();
            builder.HasMany(p => p.Posts).WithOne(u => u.User).HasPrincipalKey(p => p.Id);
            builder.HasMany(c => c.Comments).WithOne(u => u.User).HasPrincipalKey(c => c.Id);
            builder.Property(p => p.Avatar).HasMaxLength(2000);

            builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).HasPrincipalKey(r => r.Id);
            builder.Property(u => u.RoleId); //1 - User Role
        }
    }
}
