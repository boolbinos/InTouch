using System.Collections.Generic;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using System;
using System.Linq;

namespace Materialise.InTouch.BLL.Mappers
{
    public static class UserMapper
    {
        public static UserDTO ConvertToUserDTO(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsDeleted = user.IsDeleted,
                Email = user.Email,
                RoleId = user.RoleId,
                Avatar = user.Avatar
            };

            return userDTO;
        }

        public static User ConvertToUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }
            var user = new User
            {
                Id = userDTO.Id,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                IsDeleted = userDTO.IsDeleted,
                Email = userDTO.Email,
                RoleId = userDTO.RoleId,
                Avatar = userDTO.Avatar
            };

            return user;
        }

        public static List<UserDTO> ConvertToUserDTOCollection(ICollection<User> users)
        {
            if (users==null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            var usersDTO = users.Select(ConvertToUserDTO).ToList();

            return usersDTO;
        }

        public static List<User> ConvertToUserCollection(ICollection<UserDTO> usersDTO)
        {
            if (usersDTO==null)
            {
                throw new ArgumentNullException(nameof(usersDTO));
            }

            var users = usersDTO.Select(ConvertToUser).ToList();

            return users;
        }
        public static User ConvertToCreateUser(CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO));
            }
            var user = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                RoleId = Role.UserRoleId,
                Avatar = userDTO.Avatar
            };

            return user;
        }
    }
}