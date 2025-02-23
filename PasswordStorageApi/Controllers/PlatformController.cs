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
        //Get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformModel?>>> GetAllPlatforms()
        {
            try
            {
                var platforms = await _platformService.GetAllPlatforms();
                if(platforms == null)
                {
                    return NotFound();
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
                    return NotFound();
                }
                return Ok(platform);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("AddPlatform")]
        public async Task<ActionResult<PlatformModel>> AddPlatform([FromBody]PlatformModel platform)
        {
            try
            {
                var newPlatform = await _platformService.AddPlatform(platform);
                return CreatedAtAction(nameof(GetPlatformById), new { platformId = platform.PlatformId }, platform);
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
    }
}
