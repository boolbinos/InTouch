using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests.Mappers.Tests
{
    public class RoleMapperTest
    {
        [Fact]
        public void ConvertToRoleDTO_should_throw_argument_null_exception_when_input_is_null()
        {
            //arrange
            Role inputRole = null;

            //act
            var exception = Assert.Throws<ArgumentNullException>(() => 
                RoleMapper.ConvertToRoleDTO(inputRole));
        }

        [Fact]
        public void ConvertToRoleDTO_should_map_Role_to_RoleDTO()
        {
            //arrange
            Role inputRole = new Role
            {
                Id = 1,
                Name = "DefaultRole"
            };

            //act
            var resultRoleDTO = RoleMapper.ConvertToRoleDTO(inputRole);

            //assert
            Assert.Equal(inputRole.Id, resultRoleDTO.Id);
            Assert.Equal(inputRole.Name, resultRoleDTO.Name);
        }

        [Fact]
        public void ConvertToRole_should_throw_argument_null_exception_when_input_is_null()
        {
            //arrange
            RoleDTO inputRoleDTO = null;

            //act
            var exception = Assert.Throws<ArgumentNullException>(() =>
                RoleMapper.ConvertToRole(inputRoleDTO));
        }

        [Fact]
        public void ConvertToRole_should_map_RoleDTO_to_Role()
        {
            //arrange
            RoleDTO inputRoleDTO = new RoleDTO
            {
                Id = 1,
                Name = "DefaultRole"
            };

            //act
            var resultRole = RoleMapper.ConvertToRole(inputRoleDTO);

            //assert
            Assert.Equal(inputRoleDTO.Id, resultRole.Id);
            Assert.Equal(inputRoleDTO.Name, resultRole.Name);
        }
    }
}
