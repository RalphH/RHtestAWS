using s3testappapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace s3testappapi.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
    }
}
