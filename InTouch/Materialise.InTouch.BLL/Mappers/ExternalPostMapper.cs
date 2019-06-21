using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models;
using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint.Models;

namespace Materialise.InTouch.BLL.Mappers
{
    public static class ExternalPostMapper
    {
        public static Post ConvertFacebookPostToPost(FacebookPost fbPost, string defaultTitle)
        {
            if (fbPost == null)
            {
                throw new ArgumentNullException(nameof(fbPost));
            }

            return new Post
            {
                Title = fbPost.Story ?? defaultTitle,
                Content = "<p>" + fbPost.Message + "</p>",
                CreatedDate = DateTime.Parse(fbPost.Created_time),
                IsPublic = true,
                PostType = PostType.Facebook,
                UserId = 1,
                DurationInSeconds = 10,
                IsDeleted = false,
                PostFiles = new List<PostFile>(),
                Priority = PostPriority.Normal
            };
        }
        public static Post ConvertSharePointPostToPost(SharepointPost spPost)
        {
            if (spPost == null)
            {
                throw new ArgumentNullException(nameof(spPost));
            }

            var post = new Post
            {
                Title = spPost.Title,
                Content = spPost.Body,
                CreatedDate = spPost.Created,
                IsPublic = false,
                PostType = PostType.Sharepoint,
                DurationInSeconds = 10,                
                UserId = 1,
                IsDeleted = false,
                PostFiles = new List<PostFile>(),
                Priority = PostPriority.Normal  
            };

            return post;
        }
    }
}
