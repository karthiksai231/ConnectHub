using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ConnectHub.API.Data;
using ConnectHub.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectHub.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConnectHubRepository _hubRepo;
        private readonly IMapper _mapper;
        public UsersController(IConnectHubRepository hubRepo, IMapper mapper)
        {
            _mapper = mapper;
            _hubRepo = hubRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _hubRepo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(int id)
        {
            var user = await _hubRepo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var user = await _hubRepo.GetUser(id);

            _mapper.Map(userForUpdateDto, user);

            if(await _hubRepo.SaveAll()) {
                return NoContent();
            }

            throw new Exception($"Failed to update user: {id}");
        }

    }
}