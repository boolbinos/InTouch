using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();

        //Task<IEnumerable<UserDTO>> GetPageAsync(int page, int pageSize);
        Task<UserDTO> GetAsync(int id);

        Task<IEnumerable<UserDTO>> FindValidAsync(Expression<Func<User, bool>> predicate);

        Task DeleteAsync(int id);

        Task<UserDTO> CreateAsync(CreateUserDTO user);

        Task<UserDTO> Update(UserDTO user);

        Task<UserDTO> Update(UserDTO user, int id);

        Task AssignRoleAsync(int userId, string roleName);

        Task<UserDTO> GetByEmailAsync(string email);
    }
}