using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class LocalImageRepository : IImageRepository
{
    private readonly NZWalksDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _accessor;

    public LocalImageRepository(
        NZWalksDbContext context,
        IWebHostEnvironment environment, 
        IHttpContextAccessor accessor)
    {
        _context = context;
        _environment = environment;
        _accessor = accessor;
    }
    
    public async Task<Image> UploadAsync(Image image)
    {
        var localFilePath = Path.Combine(_environment.ContentRootPath, 
            "Images", $"{image.FileName}{image.FileExtension}");

        // Uploading
        using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);
        
        // https://localhost:1234/images
        // GET https://localhost:1234
        var urlFilePath = $"{_accessor.HttpContext.Request.Scheme}://" +
                          $"{_accessor.HttpContext.Request.Host}" +
                          $"{_accessor.HttpContext.Request.PathBase}/" +
                          $"Images/{image.FileName}{image.FileExtension}";

        image.FilePath = urlFilePath;
        
            // Saving
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();

        return image;
    }
}