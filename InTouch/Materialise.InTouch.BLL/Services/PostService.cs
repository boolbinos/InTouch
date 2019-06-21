using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Configs;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders;
using Materialise.InTouch.BLL.Services.Storage;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Materialise.InTouch.BLL.Enumes;
using Materialise.InTouch.BLL.Services.PostConfiguration;
using Materialise.InTouch.BLL.Services.Exceptions;

namespace Materialise.InTouch.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _db;
        private readonly IUserContext _userContext;
        private readonly IOptionsSnapshot<FullscreenBatchConfig> _batchOptions;
        private readonly IOptionsSnapshot<PostConfig> _postDefaultValuesOptions;
        private readonly IFileService _fileService;
        private readonly ICommentService _commentService;


        public PostService(IUnitOfWork uow, IFileService fileService, ICommentService commentService, IUserContext userContext,
            IOptionsSnapshot<FullscreenBatchConfig> batchOptions, IOptionsSnapshot<PostConfig> postDefaultValuesOptions)
        {
            _db = uow;
            _fileService = fileService;
            _commentService = commentService;
            _userContext = userContext;
            _postDefaultValuesOptions = postDefaultValuesOptions;
            _batchOptions = batchOptions;
        }

        public async Task<IEnumerable<PostDTO>> GetPostsAsync()
        {
            var posts = await _db.Posts.GetAllAsync();
            var postsDTO = new List<PostDTO>();
            //var currentDateTime = DateTime.Now;

            posts.ForEach(p =>
            {
                var postDTO = PostMapper.ConvertToPostDTO(p);

                postDTO.Files = FileMapper.ConvertToFileInfoDTOCollection(p.PostFiles);
                postsDTO.Add(postDTO);
            });
            return postsDTO;
        }
        public async Task<PagedResultDTO<PostDTO>> GetPageAsync(int id, int page, int pageSize, bool justMyPosts, string srchStr = null)
        {
            var pagedResult = await _db.Posts.GetPageAsync(id, page, pageSize, justMyPosts, srchStr);
            pagedResult.Data.ForEach(p =>
                p.PostFiles = p.PostFiles.Count > 0 ?
                new List<PostFile>() { p.PostFiles.First() }
                : new List<PostFile>()
            );

            Func<Post, PostDTO> func = post =>
            {
                var postDTO = PostMapper.ConvertToPostDTO(post);
                postDTO.Files = FileMapper.ConvertToFileInfoDTOCollection(post.PostFiles);
                postDTO.UserDTO = UserMapper.ConvertToUserDTO(post.User);
                postDTO.CommentsDTO = CommentMapper.ConvertToCommentDTOCollection(post.Comments);
                var usersDTOLikes = new List<UserDTO>();
                post.PostLikes.ToList().ForEach(pl =>
                {
                    var userDTO = UserMapper.ConvertToUserDTO(pl.User);
                    userDTO.RoleDTO = RoleMapper.ConvertToRoleDTO(pl.User.Role);
                    usersDTOLikes.Add(userDTO);
                });
                postDTO.UsersLikes = usersDTOLikes;
                return postDTO;
            };

            var pageResultDTO = PagedResultMapper.MapToDTO(pagedResult, func);
            return pageResultDTO;
        }

        public async Task<PostDTO> GetAsync(int id)
        {
            var post = await _db.Posts.GetAsync(id);
            if (post == null)
            { 
                throw new NotFoundException("Post is not found!");
            }
            var postDTO = PostMapper.ConvertToPostDTO(post);
            postDTO.UserDTO = UserMapper.ConvertToUserDTO(post.User);
            postDTO.UserDTO.RoleDTO = RoleMapper.ConvertToRoleDTO(post.User.Role);
            
            return postDTO;
        }

        public async Task<IEnumerable<PostDTO>> FindValidAsync(Expression<Func<Post, bool>> predicate)
        {
            var posts = await _db.Posts.FindValidAsync(predicate);
            var postsDTO = PostMapper.ConvertToPostDTOCollection(posts);
            return postsDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _db.Posts.GetAsync(id);
            if (post != null)
            {
                foreach (var pf in post.PostFiles)
                {
                    await _fileService.DeleteAsync(pf.File.Id);
                    pf.IsDeleted = true;
                }
                var comments = await _commentService.FindValidAsync(c => c.PostId == post.Id);
                foreach(var comment in comments)
                {
                    await _commentService.DeleteAsync(comment.Id);
                }
            }
            var isDeleted = await _db.Posts.DeleteAsync(id);
            await _db.SaveAsync();

            return isDeleted;
        }
        public async Task<PostDTO> CreateAsync(PostDTO post)
        {
            if (post.EndDateTime <= post.StartDateTime)
            {
                throw new InvalidOperationException("EndDateTime should be greater than StartDateTime");
            }

            var currentUser = _userContext.CurrentUser;

            if (currentUser.RoleId != (int)Roles.Moderator)
            {
                var defaultEndDateTime = _postDefaultValuesOptions.Value.DisplayPeriodInDaysForUsers;
                post.EndDateTime = post.StartDateTime.AddDays(defaultEndDateTime);
            }

            if (post.CreatedDate == default(DateTime))
            {
                post.CreatedDate = DateTime.Now;
            }
            if (post.PostType == PostType.None)
            {
                post.PostType = PostType.InTouch;
            }
            post.IsDeleted = false;


            post.UserId = currentUser.Id;

            if (post.Priority == PostPriority.High && currentUser.RoleId != Role.ModeratorRoleId)
            {
                throw new InvalidOperationException("This user has no rights to set high priority for the post");
            }

            post.IsPublic = false;
            
            var postToSave = PostMapper.ConvertToPost(post);
            postToSave.PostFiles = FileMapper.ConvertToPostFileCollection(post.Files);

            var createdPost = await _db.Posts.CreateAsync(postToSave);
            await _db.SaveAsync();

            var createdPostDTO = PostMapper.ConvertToPostDTO(createdPost);
            createdPostDTO.UserDTO = currentUser;
            return createdPostDTO;
        }
        public async Task<PostDTO> Update(int postId, EditPostDTO editPost)
        {
            if (editPost.EndDate <= editPost.StartDate)
            {
                throw new InvalidOperationException("EndDateTime should be greater than StartDateTime");
            }

            var currentUser = _userContext.CurrentUser;

            if (currentUser.RoleId != (int)Roles.Moderator)
            {
                var defaultEndDateTime = _postDefaultValuesOptions.Value.DisplayPeriodInDaysForUsers;
                editPost.EndDate = editPost.StartDate.AddDays(defaultEndDateTime);
            }

            var oldEntity = await _db.Posts.GetAsync(postId);
            if (oldEntity == null)
            {
                throw new InvalidOperationException($"Post by id: {postId} not found.");
            }
            
            if (oldEntity.Priority == PostPriority.Normal && editPost.Priority != oldEntity.Priority && currentUser.RoleId != Role.ModeratorRoleId)
            {
                throw new InvalidOperationException("This user has no rights to set high priority for the post");
            }

            oldEntity.Title = editPost.Title;
            oldEntity.Content = editPost.Content;
            oldEntity.DurationInSeconds = editPost.DurationInSeconds;
            oldEntity.VideoUrl = editPost.VideoUrl;
            oldEntity.ModifiedByUserId = _userContext.CurrentUser.Id;
            oldEntity.ModifiedDate = DateTime.Now;
            oldEntity.Priority = editPost.Priority;
            oldEntity.StartDateTime = editPost.StartDate;
            oldEntity.EndDateTime = editPost.EndDate;

            if (currentUser.RoleDTO.Name == "User" && oldEntity.IsPublic)
            {
                oldEntity.IsPublic = false;
            }

            var newFiles = FileMapper.ConvertToPostFileCollection(editPost.Files);
            var filesToRemove = (await _fileService.GetFilesForPost(oldEntity.Id))
                .Where(oldfile => !newFiles.Exists(newFile => newFile.File.Id == oldfile.Id)).ToList();

            foreach (var file in filesToRemove)
            {
                await _fileService.DeleteAsync(file.Id);
            }

            oldEntity.PostFiles = newFiles;

            var updatedPost = await _db.Posts.Update(oldEntity);
            await _db.SaveAsync();

            var updatedPostDTO = PostMapper.ConvertToPostDTO(updatedPost);
            updatedPostDTO.UserDTO = UserMapper.ConvertToUserDTO(updatedPost.User);

            return updatedPostDTO;
        }
        public async Task<bool> DoPublic(int id)
        {
            var post = await _db.Posts.GetAsync(id);
            if (post != null)
            {
                post.IsPublic = true;
                try
                {
                    await _db.SaveAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public async Task<bool> DoPrivate(int id)
        {
            var post = await _db.Posts.GetAsync(id);
            if (post != null)
            {
                post.IsPublic = false;
                try
                {
                    await _db.SaveAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        
        public async Task<List<PostDTO>> GetPostsForBatchAsync(DateTime? lastBatchDate, DateTime? lastPostCreateDate)
        {
            lastBatchDate = lastBatchDate ?? default(DateTime);
            lastPostCreateDate = lastPostCreateDate ?? default(DateTime);

            Func<Post, bool> isActual = (post) => post.IsPublic &&post.StartDateTime < DateTime.Now
                                                && post.EndDateTime > DateTime.Now;

            var priorityPosts = (await FindValidAsync(priotyPost =>
                    priotyPost.Priority == PostPriority.High
                    && isActual(priotyPost))).OrderByDescending(p => p.CreatedDate);

            var newPosts = (await FindValidAsync(newPost =>
                newPost.Priority == PostPriority.Normal
                && newPost.CreatedDate > lastBatchDate
                && isActual(newPost))).OrderByDescending(p => p.CreatedDate);

            async Task<List<PostDTO>> GetStandartPosts(bool areAlreadyShown)
            {
                var result = (await FindValidAsync(standartPost =>
                    standartPost.Priority == PostPriority.Normal
                    && standartPost.CreatedDate < lastBatchDate
                   
                    && isActual(standartPost))).OrderByDescending(p => p.CreatedDate).ToList();

                if (areAlreadyShown)
                {
                    return result;
                }
                else
                {
                    return result.Where(post => post.CreatedDate < lastPostCreateDate).ToList();
                }
            }

            var standartNotShowedPosts = await GetStandartPosts(false);

            var batchSize = _batchOptions.Value.FullscreenBatchSize;

            var posts = priorityPosts
                .Union(newPosts)
                .Union(standartNotShowedPosts)
                .Take(batchSize).ToList();

            var notEnoughtPostsForCompleteBatch = posts.Count < batchSize;

            if (notEnoughtPostsForCompleteBatch)
            {
                var postLeftToFullBatch = batchSize - posts.Count;
                var extraPosts = (await GetStandartPosts(true)).Take(postLeftToFullBatch);

                posts = posts.Union(extraPosts).ToList();
            }

            return posts;
        }
    }
}