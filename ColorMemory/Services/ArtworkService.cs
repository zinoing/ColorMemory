using Microsoft.EntityFrameworkCore;
using ColorMemory.Data;

namespace ColorMemory.Services
{
    public class ArtworkService
    {
        private readonly GameDbContext _context;

        private readonly string _s3BucketName;

        public ArtworkService(GameDbContext context, IConfiguration configuration)
        {
            _context = context;
            _s3BucketName = configuration["AWS:S3BucketName"];
        }

        public async Task<Artwork> AddArtworkAsync(string fileName)
        {
            var existingArtwork = await _context.Artworks
                .FirstOrDefaultAsync(a => a.FileName == fileName);

            if (existingArtwork != null)
            {
                return null;
            }

            if (fileName.EndsWith(".json"))
            {
                fileName = fileName[..^5];
            }

            if (fileName.EndsWith(".jpg"))
            {
                fileName = fileName[..^4];
            }

            int byIndex = fileName.IndexOf(" by ");
            if (byIndex == -1)
            {
                throw new ArgumentException("Invalid file name format. Expected format: '<Title> by <Artist>.json'");
            }

            string title = fileName[..byIndex].Trim();
            string artist = fileName[(byIndex + 4)..].Trim();
            string jsonFileUrl = $"https://{_s3BucketName}.s3.ap-northeast-2.amazonaws.com/json/{fileName}";
            string jpgFileUrl = $"https://{_s3BucketName}.s3.ap-northeast-2.amazonaws.com/jpg/{fileName}";
            var artwork = new Artwork
            {
                Title = title,
                Artist = artist,
                S3JsonUrl = jsonFileUrl,
                S3JpgUrl = jpgFileUrl,
                FileName = fileName,
            };

            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();

            return artwork;
        }
    }

}