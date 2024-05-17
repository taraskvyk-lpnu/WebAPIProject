using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

//https://localhost:xxxxx/api/regions
[ApiController]
[Route("api/[controller]")]
public class RegionsController : Controller
{
    private readonly NZWalksDbContext _dbContext;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RegionsController> _logger;

    public RegionsController(
        NZWalksDbContext dbContext,
        IRegionRepository regionRepository,
        IMapper mapper,
        ILogger<RegionsController> logger)
    {
        _dbContext = dbContext;
        _regionRepository = regionRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    // GET all regions
    // GET https://localhost:xxxxx/api/regions
    [HttpGet]
    //[Authorize(Roles = "Reader")]
    public async Task<ActionResult> GetAll()
    {
        _logger.LogInformation("GetAll Regions Action Method was invoked");
        
        // get data from db = Domain Models
        var regionsDomain = await _regionRepository.GetAllAsync();
        
        // map Domain Models to DTOs
        var regionsDto = _mapper.Map<List<RegionDTO>>(regionsDomain);
        
        _logger.LogInformation($"Finished GetAll Regions Method with data : {JsonSerializer.Serialize(regionsDomain)}");
        
        // return DTOs
        return Ok(regionsDto);
    }

    // GET REGION BY ID
    // GET https://localhost:xxxxx/api/regions/id
    [HttpGet/*("{id:guid}")*/]
    [Route("{id:guid}")]
    //[Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // get data from db = Region Domain Model
        var regionDomain = await _regionRepository.GetByIdAsync(id);

        if (regionDomain == null)
            return NotFound();

        //Mapping 
        var regionDto = _mapper.Map<RegionDTO>(regionDomain);
        
        // return Dto
        return Ok(regionDto);
    }
    
    // POST To Create New Region
    //https://localhost:xxxxx/api/regions
    [HttpPost]
    [ValidateModel]
    //[Authorize(Roles = "Writer")]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionRequestDto)
    {
        // Map DTO to Domain Model
        var regionDomain = _mapper.Map<Region>(regionRequestDto);
        await _regionRepository.CreateAsync(regionDomain);

        // Map Domain Model to DTO
        var regionDto = _mapper.Map<RegionDTO>(regionDomain);
        
        return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
    }
    
    // PUT Update Region
    //https://localhost:xxxxx/api/regions/id
    [HttpPut]
    [Route("{id:guid}")]
    [ValidateModel]
    //[Authorize(Roles = "Writer")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Map DRO to DOmain Model
        var regionDomain = _mapper.Map<Region>(updateRegionRequestDto);
        
        regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);
        
        if (regionDomain == null)
        {
            return NotFound();
        }
        
        // Convert Domain Model to DTO
        var regionDto = _mapper.Map<RegionDTO>(regionDomain);

        return Ok(regionDto);
    }

    // DELETE Domain Region
    //https://localhost:xxxxx/api/regions/id
    [HttpDelete]
    [Route("{id:guid}")]
    //[Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var regionDomain = await _regionRepository.DeleteAsync(id);

        if (regionDomain == null)
        {
            return NotFound();
        }

        await _regionRepository.DeleteAsync(regionDomain.Id);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}