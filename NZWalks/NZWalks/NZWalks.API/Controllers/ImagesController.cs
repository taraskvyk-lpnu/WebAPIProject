using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : Controller
{
    private readonly IMapper _mapper;
    private readonly IImageRepository _imageRepository;
    
    public ImagesController(IImageRepository imageRepository, IMapper mapper)
    {
        _imageRepository = imageRepository;
        _mapper = mapper;
    }
    
    // POST
    // /api/images/upload
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);
        
        if (ModelState.IsValid)
        {
            //Convert DTO to Domain

            var image = _mapper.Map<Image>(request);
            
            var imageDomain = new Image
            {
                Id = Guid.NewGuid(),
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBites = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDecsription,
            };
            
            // User repository to upload image
            await _imageRepository.UploadAsync(imageDomain);

            return Ok(imageDomain);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        var allowedExtensions = new string[] { ".svg", ".jpg", ".png" };

        if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
        {
            ModelState.AddModelError("File", "Unsupported file extension");
        }

        if (request.File.Length > 10485760)
        {
            ModelState.AddModelError("File", "File size more than 10 MB");
        }
    }
}