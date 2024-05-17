using AutoMapper;
using NZWalks.API.Models;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Region, RegionDTO>()
            //.ForMember(rd => rd.Code, opt => opt.MapFrom(r => r.Code)) // if values names where different 
            .ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        
        CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
        CreateMap<Walk,WalkDto>().ReverseMap();
        CreateMap<UpdateWalkRequest, Walk>().ReverseMap();
        
        CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        CreateMap<ImageUploadRequestDto, Image>()
            .ForMember(i => i.File, opt => opt.MapFrom(r => r.File))
            .ForMember(i => i.FileDescription, opt => opt.MapFrom(r => r.FileDecsription))
            .ForMember(i => i.FileName, opt => opt.MapFrom(r => r.FileName))
            .ForMember(i => i.FileSizeInBites, opt => opt.MapFrom(r => r.File.Length))
            .ForMember(i => i.FileExtension, opt => opt.MapFrom(r => Path.GetExtension(r.File.FileName)));
    }
}