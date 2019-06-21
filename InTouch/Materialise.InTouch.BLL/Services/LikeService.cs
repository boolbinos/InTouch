using Materialise.InTouch.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Materialise.InTouch.BLL.ModelsDTO;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using System.Linq;

namespace Materialise.InTouch.BLL.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork db;
        private readonly IUserContext _userContext;

        public LikeService(IUnitOfWork db, IUserContext userContext)
        {
            this.db = db;
            _userContext = userContext;
        }
        public async Task<List<UserDTO>> GetUsersLikes(int postId)
        {
            var users = await db.UsersLikes.GetUsersLikesForPost(postId);
            var usersDTO = users.Select(u =>
            {
                var userDTO = UserMapper.ConvertToUserDTO(u);
                userDTO.RoleDTO = RoleMapper.ConvertToRoleDTO(u.Role);
                return userDTO;
            });

            return usersDTO.ToList();
        }

        public async Task<bool> addLike(int postId, int userId)
        {
            var likes = await db.UsersLikes.GetUsersLikesForPost(postId);
            var usersLike = likes.FirstOrDefault(u => u.Id == userId);

            try
            {
                if (usersLike == null)
                {
                    await db.UsersLikes.AddLike(postId, userId);
                }
                else
                {
                    await db.UsersLikes.RemoveLike(postId, userId);
                }

                await db.SaveAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
