using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ConnectHub.API.Data;
using ConnectHub.API.Dtos;
using ConnectHub.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectHub.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var currentUser = await _hubRepo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrWhiteSpace(userParams.Gender)) {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }

            var users = await _hubRepo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, userParams.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
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