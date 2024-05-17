using System.ComponentModel.DataAnnotations;
namespace NZWalks.API.Models.DTO;

public class UpdateRegionRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 chars")]
    [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 chars")]
    public string Code { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Name has to be a minimum of 2 chars")]
    [MaxLength(40, ErrorMessage = "Name has to be a maximum of 40 chars")]
    public string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}