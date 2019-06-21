using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Services;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests.Services.Tests
{
    public class UserServiceTest
    {
        private readonly UserService _userService;

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserContext> _userContext;

        public UserServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userContext = new Mock<IUserContext>();

            var userServiceMock = new Mock<UserService>(MockBehavior.Strict);

            _userService = new UserService(_unitOfWorkMock.Object, _userContext.Object);
        }

        [Fact]
        public async Task AsignRoleAsync_should_throw_InvalidOperationException_when_assignee_try_to_change_role_for_himself()
        {
            //arrange
            var roleName = "Moderator";
            var assignee = new UserDTO() { Id = 4, Email = "mail@mail.com", FirstName = "Jon", LastName = "Smith", IsDeleted = false, RoleId = 2, RoleDTO = new RoleDTO { Id = 2, Name = "Moderator" } };
            
            _userContext.Setup(m => m.CurrentUser).Returns(assignee);
            _unitOfWorkMock.Setup(m => m.Users.GetAsync(assignee.Id)).ReturnsAsync(new User { Id = 4 });

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _userService.AssignRoleAsync(assignee.Id, roleName));

            //assert
            Assert.Equal($"Assignee cannot change role for himself", exception.Message);
        }
        
        [Fact]
        public async Task AsignRoleAsync_should_throw_InvalidOperationException_when_user_not_exist()
        {
            //arrange
            var roleName = "Moderator";
            var assignee = new UserDTO() { Id = 4, RoleId = 2 };
            var user = new User() { Id = 6 };

            _unitOfWorkMock.Setup(m => m.Users.GetAsync(user.Id)).ReturnsAsync((User)null);
            _userContext.Setup(m => m.CurrentUser).Returns(assignee);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _userService.AssignRoleAsync(assignee.Id, roleName));

            //assert
            Assert.Equal($"The user with id={assignee.Id} does not exist", exception.Message);
        }

        [Fact]
        public async Task AsignRoleAsync_should_throw_InvalidOperationException_when_assignee_is_not_moderator()
        {
            //arrange
            var roleName = "Moderator";
            var assignee = new UserDTO() { Id = 4, RoleId = 1, RoleDTO = new RoleDTO { Id = 1, Name = "User" } };
            var user = new User() { Id = 6 };

            _unitOfWorkMock.Setup(m => m.Users.GetAsync(user.Id)).ReturnsAsync(user);
            _userContext.Setup(m => m.CurrentUser).Returns(assignee);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _userService.AssignRoleAsync(user.Id, roleName));

            //assert
            Assert.Equal("The assignee has no rights to change roles", exception.Message);
        }

        [Fact]
        public async Task AsignRoleAsync_should_throw_InvalidOperationException_when_user_is_default_user()
        {
            //arrange
            var roleName = "Moderator";
            var assignee = new UserDTO() { Id = 4, RoleId = 2, RoleDTO = new RoleDTO { Id = 2, Name = "Moderator" } };
            var user = new User() { Id = 1 };

            _unitOfWorkMock.Setup(m => m.Users.GetAsync(user.Id)).ReturnsAsync(user);
            _userContext.Setup(m => m.CurrentUser).Returns(assignee);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _userService.AssignRoleAsync(user.Id, roleName));

            //assert
            Assert.Equal("The assignee has no rights to change role for default user", exception.Message);
        }

        [Fact]
        public async Task AsignRoleAsync_should_throw_InvalidOperationException_when_role_not_exist()
        {
            //arrange
            var roleName = "NotExistingRoleName";
            var assignee = new UserDTO() { Id = 4, RoleId = 2, RoleDTO = new RoleDTO { Id = 2, Name = "Moderator" } };
            var user = new User() { Id = 5 };

            _unitOfWorkMock.Setup(m => m.Users.GetAsync(user.Id)).ReturnsAsync(user);
            _userContext.Setup(m => m.CurrentUser).Returns(assignee);
            _unitOfWorkMock.Setup(m => m.Roles.GetByNameAsync(roleName)).ReturnsAsync((Role)null);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _userService.AssignRoleAsync(user.Id, roleName));

            //assert
            Assert.Equal($"The role named {roleName} does not exist", exception.Message);
        }

        [Fact]
        public async Task AsignRoleAsync_should_call_UsersUpdate_once()
        {
            //arrange
            var roleModerator = new Role { Id = 2, Name = "Moderator" };
            var roleUser = new Role { Id = 1, Name = "User" };
            var assignee = new UserDTO() { Id = 4, Email="mail@mail.com", FirstName="Jon", LastName="Smith", IsDeleted = false, RoleId = 2, RoleDTO = new RoleDTO { Id = 2, Name = "Moderator" } };
            var user = new User() { Id = 5, RoleId = 1, Role = roleUser };

            _unitOfWorkMock.Setup(m => m.Users.GetAsync(user.Id)).ReturnsAsync(user);
            _userContext.Setup(m => m.CurrentUser).Returns(assignee);
            _unitOfWorkMock.Setup(m => m.Users.Update(It.IsAny<User>())).ReturnsAsync(It.IsAny<User>());
            _unitOfWorkMock.Setup(m => m.Roles.GetByNameAsync("Moderator")).ReturnsAsync(roleModerator);

            //act
            await _userService.AssignRoleAsync(user.Id, "Moderator");

            //assert
            _unitOfWorkMock.Verify(m => m.Users.Update(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(m => m.Users.Update(It.Is<User>(u => u == user)));
        }
    }
}
