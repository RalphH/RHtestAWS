using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using s3testappapi.Services;

namespace s3testappapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3bucketController : ControllerBase
    {
        private readonly IS3Service _service;

        public S3bucketController(IS3Service service)
        {
            _service = service;
        }

        [HttpPost("{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);

            return Ok(response);
        }
    }
}