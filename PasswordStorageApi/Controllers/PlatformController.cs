using Microsoft.AspNetCore.Mvc;
using PasswordStorageApi.Models;
using PasswordStorageApi.Service.Interface;
using System;

namespace PasswordStorageApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        //Get All Platform
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformModel?>>> GetAllPlatforms()
        {
            try
            {
                var platforms = await _platformService.GetAllPlatforms();
                if(!platforms.Any())
                {
                    return NotFound("No platform found !");
                }
                return Ok(platforms);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Get by id
        [HttpGet("{platformId}")]
        public async Task<ActionResult<PlatformModel?>> GetPlatformById(int platformId)
        {
            try
            {
                var platform = await _platformService.GetPlatformById(platformId);
                if (platform == null)
                {
                    return NotFound("Platform does not exist!");
                }
                return Ok(platform);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Post Add Platform
        [HttpPost("AddPlatform")]
        public async Task<ActionResult<PlatformModel>> AddPlatform([FromBody]PlatformModel platform)
        {
            try
            {
                var newPlatform = await _platformService.AddPlatform(platform);
                return CreatedAtAction(nameof(GetPlatformById), new { platformId = newPlatform.PlatformId }, newPlatform);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("UpdatePlatform/{platformId}")]
        public async Task<ActionResult<PlatformModel>> UpdatePlatform(int platformId, [FromBody]PlatformModel platform)
        {
            try
            {
                var updatedPlatform = await _platformService.UpdatePlatform(platformId, platform);

                if (updatedPlatform == null)
                    return NotFound("Platform does not exist!");

                return CreatedAtAction(nameof(GetPlatformById), new { platformId = updatedPlatform.PlatformId }, updatedPlatform);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("DeletePlatform/{platformId}")]
        public async Task<ActionResult<PlatformModel>> DeletePlatform(int platformId)
        {
            try
            {
                var deletePlatform = await _platformService.DeletePlatform(platformId);
                if (deletePlatform == null)
                    return NotFound("Platform does not exist");
                return Ok("Successfully Deleted");
            }
            catch (HttpRequestException htex)
            {
                return BadRequest(new { Message = htex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest($"Unable to delete item: {ex.Message}");
            }

        }
    }
}
