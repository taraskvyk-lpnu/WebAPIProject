using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalksController : Controller
{
    private readonly IMapper _mapper;
    private readonly IWalkRepository _walkRepository;

    public WalksController(IWalkRepository walkRepository, IMapper mapper)
    {
        _mapper = mapper;
        _walkRepository = walkRepository;
    }
    
    // CREATE Walk
    // POST  /api/walks
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto walkRequestDto)
    {
        // Map to Domain
        var walkDomain = _mapper.Map<Walk>(walkRequestDto);
        
        // Add
        await _walkRepository.CreateAsync(walkDomain);

        // Map to DTO
        return Ok(_mapper.Map<WalkDto>(walkDomain));
    }
    
    // GET Walks
    [HttpGet]
    // api/walks?fiterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
    public async Task<IActionResult> GetAll(
        [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
    {
        var walksDomain = await _walkRepository
            .GetAllAsync(
                filterOn, filterQuery, 
                sortBy, isAscending ?? true,
                pageNumber, pageSize);
        // Map to Dto
        return Ok(_mapper.Map<IEnumerable<WalkDto>>(walksDomain));
    }
    
    // GET Walk by id
    // api/walks/id
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomain = await _walkRepository.GetByIdAsync(id);

        if (walkDomain == null)
        {
            return BadRequest();
        }
        
        // Map to Dto
        return Ok(_mapper.Map<WalkDto>(walkDomain));
    }
    
    // Update Walk by id
    // PUT 
    // api/walks/id
    [HttpPut]
    [Route("{id:guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequest updateWalkRequest)
    {
        // Map DTO to Domain
        var walkDomain = _mapper.Map<Walk>(updateWalkRequest);

        walkDomain = await _walkRepository.UpdateAsync(id, walkDomain);

        if (walkDomain == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<WalkDto>(walkDomain));
    }
    
    // DELETE Walk by id
    // api/walks/id
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deletedWalkDomain = await _walkRepository.DeleteByIdAsync(id);

        if (deletedWalkDomain == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<WalkDto>(deletedWalkDomain));
    }
}