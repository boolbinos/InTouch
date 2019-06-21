using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.Mappers
{
    public static class RoleMapper
    {
        public static RoleDTO ConvertToRoleDTO(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleDTO = new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };

            return roleDTO;
        }

        public static Role ConvertToRole(RoleDTO roleDTO)
        {
            if (roleDTO == null)
            {
                throw new ArgumentNullException(nameof(roleDTO));
            }

            var role = new Role
            {
                Id = roleDTO.Id,
                Name = roleDTO.Name
            };

            return role;
        }
    }
}
