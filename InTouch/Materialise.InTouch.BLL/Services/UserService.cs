using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using System.Linq;

namespace Materialise.InTouch.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork db;
        private readonly IUserContext _userContext;

        public UserService(IUnitOfWork uow, IUserContext userContext)
        {
            db = uow;
            _userContext = userContext;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await db.Users.GetAllAsync();
            var usersDTO = users.Select(u =>
            {
                var userDTO = UserMapper.ConvertToUserDTO(u);
                userDTO.RoleDTO = RoleMapper.ConvertToRoleDTO(u.Role);
                return userDTO;
            });
            return usersDTO;
        }

        public Task<PagedResultDTO<UserDTO>> GetPageAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
            //var pagedResult = await db.Post.GetPageAsync(page, pageSize);
            //UserDTO UserDtoMapper(User user) => UserMapper.ConvertToUserDTO(user);
            //return PagedResultMapper.MapToDTO(pagedResult, UserDtoMapper);
        }

        public async Task<UserDTO> GetAsync(int id)
        {
            var user = await db.Users.GetAsync(id);
            var userDTO = UserMapper.ConvertToUserDTO(user);
            return userDTO;
        }

        public async Task<IEnumerable<UserDTO>> FindValidAsync(Expression<Func<User, bool>> predicate)
        {
            var users = await db.Users.FindValidAsync(predicate);
            var usersDTO = users.Select(u =>
            {
                var userDTO = UserMapper.ConvertToUserDTO(u);
                userDTO.RoleDTO = RoleMapper.ConvertToRoleDTO(u.Role);
                return userDTO;
            });

            return usersDTO;
        }

        public async Task DeleteAsync(int id)
        {
            await db.Users.DeleteAsync(id);
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO userDTO)
        {
            var newUser = UserMapper.ConvertToCreateUser(userDTO);

            var createdUser = await db.Users.CreateAsync(newUser);
            await db.SaveAsync();

            var createdUserDTO = UserMapper.ConvertToUserDTO(createdUser);
            return createdUserDTO;
        }

        public async Task<UserDTO> Update(UserDTO userDTO)
        {
            var updatedUser = await db.Users.Update(UserMapper.ConvertToUser(userDTO));
            await db.SaveAsync();
            var updatedUserDTO = UserMapper.ConvertToUserDTO(updatedUser);
            return updatedUserDTO;
        }

        public async Task<UserDTO> Update(UserDTO user, int id)
        {
            user.Id = id;
            return await Update(user);
        }

        public async Task AssignRoleAsync(int userId, string roleName)
        {
            var assignee = UserMapper.ConvertToUser(_userContext.CurrentUser);
            var user = await db.Users.GetAsync(userId);
            
            if (user == null)
            {
                throw new InvalidOperationException($"The user with id={userId} does not exist");
            }

            CheckPermissions(assignee, user);

            var role = await db.Roles.GetByNameAsync(roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"The role named {roleName} does not exist");
            }
            user.RoleId = role.Id;

            await db.Users.Update(user);
            await db.SaveAsync();
        }

        private void CheckPermissions(User assignee, User user)
        {
            if (assignee.Id == user.Id)
            {
                throw new InvalidOperationException($"Assignee cannot change role for himself");
            }

            if (!assignee.IsModerator())
            {
                throw new InvalidOperationException("The assignee has no rights to change roles");
            }

            if (user.IsDefaultUser())
            {
                throw new InvalidOperationException("The assignee has no rights to change role for default user");
            }
        }

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(nameof(email));
            }

            var user = await db.Users.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var userDto = UserMapper.ConvertToUserDTO(user);
            userDto.RoleDTO = RoleMapper.ConvertToRoleDTO(user.Role);

            return userDto;
        }
    }
}