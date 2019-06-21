using Materialise.InTouch.BLL.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Materialise.InTouch.BLL.ModelsDTO;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Configs;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Microsoft.Extensions.Options;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _db;
        private readonly IUserContext _userContext;
        private readonly IOptionsSnapshot<LatestCommentsConfig> _latestCommentsOptions;

        public CommentService(IUnitOfWork uow, IUserContext userContext, IOptionsSnapshot<LatestCommentsConfig> latestCommentsOptions)
        {
            _db = uow;
            _userContext = userContext;
            _latestCommentsOptions = latestCommentsOptions;
        }

        public async Task<CommentDTO> CreateAsync(CommentDTO comment)
        {
            var currentUser = _userContext.CurrentUser;

            if (comment.CreatedDate == default(DateTime))
            {
                comment.CreatedDate = DateTime.Now;
            }
            comment.UserId = currentUser.Id;

            var commentToSave = CommentMapper.ConvertToComment(comment);

            var createdComment = await _db.Comments.CreateAsync(commentToSave);
            await _db.SaveAsync();

            var createdCommentDTO = CommentMapper.ConvertToCommentDTO(createdComment);
            createdCommentDTO.UserDTO = currentUser;
            return createdCommentDTO;

        }

        public async Task<CommentDTO> GetCommentAsync(int id)
        {
            var comment = await _db.Comments.GetAsync(id);
            var commentDTO = CommentMapper.ConvertToCommentDTO(comment);
            commentDTO.UserDTO = UserMapper.ConvertToUserDTO(comment.User);

            return commentDTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync()
        {
            var comments = await _db.Comments.GetAllValidAsync();

            var commentsDTO = comments.Select(u =>
            {
                var commentDTO = CommentMapper.ConvertToCommentDTO(u);
                commentDTO.UserDTO = UserMapper.ConvertToUserDTO(u.User);

                return commentDTO;
            });
            return commentsDTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByPostAsync(int postId)
        {
            var comments = await _db.Comments.GetAllByPost(postId);

            var commentsDTO = comments.Select(c =>
            {
                var commentDTO = CommentMapper.ConvertToCommentDTO(c);
                commentDTO.UserDTO = UserMapper.ConvertToUserDTO(c.User);
                return commentDTO;
            });
            return commentsDTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByUserAsync(int userId)
        {
            var comments = await _db.Comments.GetAllByUser(userId);

            var commentsDTO = comments.Select(c =>
            {
                var commentDTO = CommentMapper.ConvertToCommentDTO(c);
                //commentDTO.UserDTO = UserMapper.ConvertToUserDTO(c.User);
                //commentDTO.PostDTO = PostMapper.ConvertToPostDTO(c.Post);
                return commentDTO;
            });
            return commentsDTO;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isDeleted = await _db.Comments.DeleteAsync(id);
            await _db.SaveAsync();

            return isDeleted;
        }

        public async Task<IEnumerable<LatestCommentDTO>> GetLatestComments()
        {
            var latestCommentsDTO = (await _db.Comments.GetAllValidAsync())
                                    .Where(comment => comment.Post.IsDeleted == false)
                                    .Select(CommentMapper.ConvertToLatestCommentDTO)
                                    .Take(_latestCommentsOptions.Value.NumberOfLatestCommentsToShow);
            return latestCommentsDTO;
        }

        public async Task<IEnumerable<CommentDTO>> FindValidAsync(Expression<Func<Comment, bool>> predicate)
        {
            return (await _db.Comments.FindValidAsync(predicate)).Select(c =>
            {
                CommentDTO commentDTO = CommentMapper.ConvertToCommentDTO(c);
                commentDTO.UserDTO = UserMapper.ConvertToUserDTO(c.User);
                return commentDTO;
            });
        }
    }
}
