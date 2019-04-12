using System.Threading.Tasks;
using ConnectHub.API.Data;
using ConnectHub.API.Dtos;
using ConnectHub.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            if (await _authRepository.UserExists(userForRegisterDto.UserName))
            {
                return BadRequest("User already exists");
            }

            var userToCreate = new User
            {
                UserName = userForRegisterDto.UserName 
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }
    }
}