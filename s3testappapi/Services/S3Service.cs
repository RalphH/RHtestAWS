using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using s3testappapi.Models;

namespace s3testappapi.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;
        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            var res = new S3Response
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Error"              
            };

            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    //relies on credentials C:\Users\<user>\.aws (access key in file & secret key can only be seen when creating access key 1st time via AWS console) 
                    var response = await _client.PutBucketAsync(putBucketRequest);

                    res.Status = response.HttpStatusCode;
                    res.Message = response.ResponseMetadata.RequestId;
                }
                else
                {
                    res.Status = HttpStatusCode.Conflict;
                    res.Message = string.Format("Already exists : {0}", bucketName);
                }
            }
            catch (AmazonS3Exception exaws)
            {
                Console.WriteLine(exaws);
                res.Status = exaws.StatusCode;
                res.Message = exaws.Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                res.Status = HttpStatusCode.InternalServerError;
                res.Message = ex.Message;
            }

            return res;
        }

        private const string FilePath = "c:\\temp\\test.jpg";
        private const string UploadWithKeyName = "UploadWithKeyName";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvancedUpload";

        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);

                //option1
                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                //option2
                await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithKeyName);

                //option3
                using (var fileToUpload = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, FileStreamUpload);
                }

                //option4
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = FilePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456, //6mb
                    Key = AdvancedUpload,
                    CannedACL = S3CannedACL.NoACL
                };

                fileTransferUtilityRequest.Metadata.Add("Param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("Param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception exaws)
            {
                Console.WriteLine(exaws.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
