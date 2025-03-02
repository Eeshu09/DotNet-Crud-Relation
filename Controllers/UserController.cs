using Crud.Interface;
using Crud.Model;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IHangFireJobs _hanFireJobs;

        public UserController(IUser userService ,SieveProcessor sieveProcessor,IBackgroundJobClient backgroundJobClient,IHangFireJobs hangFireJobs)
        {
            _userService=userService;
            _sieveProcessor=sieveProcessor;
            _backgroundJobClient = backgroundJobClient;
            _hanFireJobs = hangFireJobs;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUser([FromQuery] SieveModel model)
        {
            var users = await  _userService.GetAllUsersQuery();
            var filters=_sieveProcessor.Apply(model, users);  
            return Ok(filters);

            //users=_sieveProcessor.Apply(/*model*/, users);
            //return Ok(users);
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
