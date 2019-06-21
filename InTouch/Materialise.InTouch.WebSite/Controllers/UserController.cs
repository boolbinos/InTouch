using Materialise.InTouch.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Materialise.InTouch.WebSite.Mappers;
using Materialise.InTouch.WebSite.Authorize;
using System;
using System.Linq;
using Materialise.InTouch.BLL.ModelsDTO;
using Microsoft.AspNetCore.Authorization;

namespace Materialise.InTouch.WebSite.Controllers
{
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private IUserService _userService;
        private IUserContext _userContext;

        public UserController(IUserService userService, IUserContext userContext)
        {
            _userService = userService;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Policy = "RequireModeratorRole")]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not valid data");
            }

            var usersDTO = (await _userService.GetUsersAsync()).OrderByDescending(u => u.RoleId).ThenBy(u => u.FirstName);

            var users = UserMapper.ConvertToUserViewModelCollection(usersDTO);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _userService.GetAsync(id);
            var userViewModel = UserMapper.ConvertToUserViewModel(user);

            return Ok(userViewModel);
        }

        [HttpPost]
        [Route("assignRole")]
        public async Task<IActionResult> AssignRole(int userId, string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not valid data");
            }

            await _userService.AssignRoleAsync(userId, roleName);

            return Ok();
        }

        [HttpGet]
        //[Authorize(Policy = "RequireModeratorRole")]
        [Route("GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
                var user =  _userContext.CurrentUser;
                var viewUser = UserMapper.ConvertToUserViewModel(user);
                return Ok(viewUser);         
        }
    }
}
