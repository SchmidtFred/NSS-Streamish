using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Streamish.Repositories;
using Streamish.Models;

namespace Streamish.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _profileRepository;
        public UserProfileController(IUserProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_profileRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var profile = _profileRepository.GetById(id);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpGet("GetProfileWithVideos/{id}")]
        public IActionResult GetProfileWithVideos(int id)
        {
            var profile = _profileRepository.GetByIdWithVideos(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpGet("{firebaseUserId}")]
        public IActionResult GetByFirebaseUserId(string firebaseUserId)
        {
            var profile = _profileRepository.GetByFirebaseUserId(firebaseUserId);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpGet("DoesUserExist/{firebaseUserId}")]
        public IActionResult DoesUserExist(string firebaseUserId)
        {
            var profile = _profileRepository.GetByFirebaseUserId(firebaseUserId);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(UserProfile profile)
        {
            _profileRepository.Add(profile);
            return CreatedAtAction("Get", new { id = profile.Id }, profile);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }

            _profileRepository.Update(profile);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _profileRepository.Delete(id);
            return NoContent();
        }
    }
}
