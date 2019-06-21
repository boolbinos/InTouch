using Materialise.InTouch.BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface ILikeService
    {
        Task<List<UserDTO>> GetUsersLikes(int postId);
        Task<bool> addLike(int postId, int userId);
    }
}
