using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ConnectHub.API.Data;
using ConnectHub.API.Dtos;
using ConnectHub.API.Helpers;
using ConnectHub.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConnectHub.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IConnectHubRepository _connectHubRepo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _options;
        private Cloudinary _cloudinary;

        public PhotosController(IConnectHubRepository connectHubRepo, IMapper mapper, IOptions<CloudinarySettings> options)
        {
            _connectHubRepo = connectHubRepo;
            _mapper = mapper;
            _options = options;

            Account acc = new Account(
                _options.Value.CloudName,
                _options.Value.ApiKey,
                _options.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _connectHubRepo.GetPhoto(id);

            var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(int userId, [FromForm]PhotoForCreationDto photoForCreation)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await _connectHubRepo.GetUser(userId);

            var file = photoForCreation.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                        .Width(500)
                        .Height(500)
                        .Crop("fill")
                        .Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreation);

            if (!user.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _connectHubRepo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Unable to Upload Photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await _connectHubRepo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photo = await _connectHubRepo.GetPhoto(id);

            if (photo.IsMain)
            {
                return BadRequest("Is already a Main Photo");
            }

            var currMain = await _connectHubRepo.GetMainPhotoForUser(userId);
            currMain.IsMain = false;

            photo.IsMain = true;

            if (await _connectHubRepo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Unable to set Main Photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = await _connectHubRepo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photo = await _connectHubRepo.GetPhoto(id);

            if (photo.IsMain)
            {
                return BadRequest("Cannot delete Main Photo");
            }

            if (!string.IsNullOrWhiteSpace(photo.PublicId))
            {
                var deleteParams = new DeletionParams(photo.PublicId);

                var destroy = _cloudinary.Destroy(deleteParams);

                if (destroy.Result == "ok")
                {
                    _connectHubRepo.Delete(photo);
                }
            } else {
                _connectHubRepo.Delete(photo);
            }

            if (await _connectHubRepo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Unable to Delete photo");
        }
    }
}