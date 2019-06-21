using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Materialise.InTouch.IntegrationTests.Common
{
    public static class TestDataInitializer
    {
        public static async Task Init_1Post(
            InTouchContext context, string title,
            int daysOld, PostPriority priority)
        {
            daysOld *= -1;

           var user =  await InitOneGeneralUserAsync(context);
            var post = new Post
            {
                Content = "Content1",
                CreatedDate = DateTime.Now.AddDays(daysOld),
                IsDeleted = false,
                IsPublic = true,
                Title = title,
                UserId = user.Id,
                Priority = priority,
                StartDateTime = DateTime.Now.AddDays(daysOld),
                EndDateTime = DateTime.Now.AddDays(Math.Abs(daysOld) + 50),
            };
            context.Posts.Add(post);
            context.SaveChanges();
        }

        public static async Task Init_3HP_3NP_Posts(InTouchContext context)
        {
            await Init_1Post(context, "NH", 8, PostPriority.High);
            await Init_1Post(context, "N", 5, PostPriority.Normal);
            await Init_1Post(context, "3", 10, PostPriority.Normal);
            await Init_1Post(context, "2", 20, PostPriority.Normal);
            await Init_1Post(context, "1", 30, PostPriority.Normal);
            await Init_1Post(context, "OH", 100, PostPriority.High);
        }

        public async static Task Init5PostsWith1DeletedPost(InTouchContext context, User user)
        {
            //var user1 = await InitOneGeneralUserAsync(context);

            var posts = new Post[]
            {
                new Post
                {
                        Content = "Content1",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsPublic = true,
                        Title = "Title1",
                        UserId = user.Id
                },
                new Post
                {
                        Content = "Content2",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsPublic = true,
                        Title = "Title2",
                        UserId = user.Id
                },
                new Post{
                        Content = "Content3",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsPublic = true,
                        Title = "Title3",
                         UserId = user.Id
                },
                new Post{
                        Content = "Content4",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsPublic = true,
                        Title = "Title4",
                        UserId = user.Id
                },
                new Post{
                        Content = "Deleted",
                        CreatedDate = DateTime.Now,
                        IsDeleted = true,
                        IsPublic = true,
                        Title = "Deleted",
                        UserId = user.Id
                }};

            foreach (var post in posts)
            {
                context.Posts.Add(post);
            }

            context.SaveChanges();
        }

        public static void InitThreeUsersWhereLastOneIsDeleted(InTouchContext context)
        {
            var users = new User[]
            {
                new User
                {
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    IsDeleted = false
                },
                new User
                {
                    FirstName = "user",
                    LastName = "user",
                    Email = "user@gmail.com",
                    IsDeleted = false
                },
                new User
                {
                    FirstName = "deleted",
                    LastName = "deleted",
                    Email = "deletedgmail.com",
                    IsDeleted = true
                }
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }

        public static void Init6UsersWhereLastOneIsDeleted(InTouchContext context)
        {
            var users = new List<User>();

            for (int i = 0; i < 5; i++)
            {
                users.Add(
                    new User
                    {
                        FirstName = "user" + i,
                        LastName = "user" + i,
                        Email = "user@gmail.com" + i,
                        IsDeleted = false,
                    });
            }

            users.Add(new User
            {
                FirstName = "deleted",
                LastName = "deleted",
                Email = "deletedgmail.com",
                IsDeleted = true,
            });

            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }

        public async static Task<User> InitOneGeneralUserAsync(InTouchContext context)
        {
            var userToAdd = context.Users.Add(new User
            {
                FirstName = "admin",
                LastName = "admin",
                Email = Guid.NewGuid() + "@m.c",
                IsDeleted = false,
                RoleId = 2
            });
            context.SaveChanges();

            var user = await context.Users.LastOrDefaultAsync();

            return user;
        }

        public async static Task<User> InitOneDeletedUserAsync(InTouchContext context)
        {
            var userToAdd = context.Users.Add(new User
            {
                FirstName = "deleted",
                LastName = "deleted",
                Email = "deleted@gmail.com",
                IsDeleted = true,
            });
            context.SaveChanges();

            var user = await context.Users.LastOrDefaultAsync();

            return user;
        }

        public async static Task<Post> InitOneDeletedPostAsync(InTouchContext context)
        {
            var user1 = await InitOneGeneralUserAsync(context);
            var postToAdd = context.Posts.Add(
                new Post
                {
                    Content = "Content1",
                    CreatedDate = DateTime.Now,
                    IsDeleted = true,
                    IsPublic = true,
                    Title = "Title1",
                    UserId = user1.Id
                });

            context.SaveChanges();

            var post = await context.Posts.LastOrDefaultAsync();

            return post;
        }


        public static void ClearData(InTouchContext context)
        {
            context.Posts.RemoveRange(context.Posts);
            context.SaveChanges();

            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
        }
    }
}