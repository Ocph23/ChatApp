﻿using Amazon.S3;
using Amazon.S3.Model;
using ChatApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private string bucketName = "ocph23";
        public FilesController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }


        [HttpGet("get-bucket")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            try
            {
                var data = await _s3Client.ListBucketsAsync();
                var buckets = data.Buckets.Select(b => { return b.BucketName; });
                return Ok(buckets);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync()
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
            await _s3Client.PutBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created.");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, string? prefix)
        {
            string fileName = System.IO.Path.GetRandomFileName() +"."+ file.FileName.Split(".")[1];
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request);
            return Ok($"File {prefix}/{fileName} uploaded to S3 successfully!");
        }

        [HttpPost("uploadgroupfile")]
        public async Task<IActionResult> UploadGroupFileAsync(int groupid, IFormFile file)
        {
            string? prefix = "";
            string fileName = System.IO.Path.GetRandomFileName() + "." + file.FileName.Split(".")[1];
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request);
            return Ok($"{prefix}/{fileName}");
        }

        [HttpPost("uploadprivatefile")]
        public async Task<IActionResult> UploadPrivateFileAsync(string temainId, IFormFile file)
        {
            string? prefix = "";
            string fileName = System.IO.Path.GetRandomFileName() + "." + file.FileName.Split(".")[1];
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request);
            return Ok($"{prefix}/{fileName}");
        }



        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllFilesAsync( string? prefix)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };
            var result = await _s3Client.ListObjectsV2Async(request);
            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                return new S3ObjectDto(s.Key.ToString(), _s3Client.GetPreSignedURL(urlRequest));
            });
            return Ok(s3Objects);
        }

        [HttpGet("get-by-key")]
        public async Task<IActionResult> GetFileByKeyAsync(string key)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
            var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
            return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
        }



        [HttpGet("get-url")]
        public async Task<IActionResult> GetUrlFileAsync(string? prefix,string key)
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };
            return Ok(new S3ObjectDto(key, _s3Client.GetPreSignedURL(urlRequest)));
        }
    }
}
