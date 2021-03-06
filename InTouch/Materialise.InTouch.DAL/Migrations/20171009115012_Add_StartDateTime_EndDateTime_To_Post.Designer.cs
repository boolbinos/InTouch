﻿// <auto-generated />
using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Materialise.InTouch.DAL.Migrations
{
    [DbContext(typeof(InTouchContext))]
    [Migration("20171009115012_Add_StartDateTime_EndDateTime_To_Post")]
    partial class Add_StartDateTime_EndDateTime_To_Post
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.FileInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SizeKB");

                    b.HasKey("Id");

                    b.ToTable("File");
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("DurationInSeconds")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(10);

                    b.Property<DateTime>("EndDateTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool?>("IsValid");

                    b.Property<int?>("ModifiedByUserId");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int>("PostType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<DateTime>("StartDateTime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("UserId");

                    b.Property<string>("VideoUrl");

                    b.HasKey("Id");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("UserId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.PostFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("FileId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("FileId")
                        .IsUnique();

                    b.HasIndex("PostId");

                    b.ToTable("PostFile");
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.PostLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PostId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostLike");
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.Post", b =>
                {
                    b.HasOne("Materialise.InTouch.DAL.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Materialise.InTouch.DAL.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.PostFile", b =>
                {
                    b.HasOne("Materialise.InTouch.DAL.Entities.FileInfo", "File")
                        .WithOne("PostFile")
                        .HasForeignKey("Materialise.InTouch.DAL.Entities.PostFile", "FileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Materialise.InTouch.DAL.Entities.Post", "Post")
                        .WithMany("PostFiles")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.PostLike", b =>
                {
                    b.HasOne("Materialise.InTouch.DAL.Entities.Post", "Post")
                        .WithMany("PostLikes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Materialise.InTouch.DAL.Entities.User", "User")
                        .WithMany("PostsLike")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Materialise.InTouch.DAL.Entities.User", b =>
                {
                    b.HasOne("Materialise.InTouch.DAL.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
