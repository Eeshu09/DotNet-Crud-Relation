using Crud.Interface;
using Crud.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        public UserController(IUser userService)
        {
            _userService=userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
        {
            var users = await _userService.GetAllUser();
            return Ok(users);
            // return Ok(_userService.GetAllUser());
        }
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserDto user)
        {

            var newuser = await _userService.AddUser(user);
            //return CreatedAtAction(nameof(GetUserById), new { id = newuser.Id }, newuser);
            return Ok(newuser);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>>DeleteUserById(int id)
        {
            var result = await _userService.DeleteUser(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            var result = await _userService.UpdateUser(id, user);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
