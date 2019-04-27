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

        [HttpPost]
        [Route("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            //postman http://localhost:<port>/api/S3bucket/CreateBucket/rhtestbucket
            var response = await _service.CreateBucketAsync(bucketName);

            return Ok(response);
        }

        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {            
            //postman http://localhost:<port>/api/S3bucket/AddFile/rhtestbucket
            await _service.UploadFileAsync(bucketName);

            return Ok();
        }
    }
}