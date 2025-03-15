using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.EntityFrameworkCore;
using ColorMemory.Data;

namespace ColorMemory.Services
{
    public class ArtworkService
    {
        private readonly GameDbContext _context;
        private readonly IAmazonS3 _s3Client;

        private readonly string _s3BucketName;

        public ArtworkService(GameDbContext context, IAmazonS3 s3Client, IConfiguration configuration)
        {
            _context = context;
            _s3Client = s3Client;
            _s3BucketName = configuration["AWS:S3BucketName"];
        }

        public async Task<Artwork> AddArtworkAsync(string title, string artist, IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string fileUrl = $"https://{_s3BucketName}.s3.amazonaws.com/{fileName}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = memoryStream,
                Key = fileName,
                BucketName = _s3BucketName,
                ContentType = file.ContentType
            };
            var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(uploadRequest);

            var artwork = new Artwork
            {
                Title = title,
                Artist = artist,
                S3Url = fileUrl,
                FileName = fileName,
                ContentType = file.ContentType
            };

            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();

            return artwork;
        }
    }

}