using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories;

public class SQLWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;
    
    public SQLWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<List<Walk>> GetAllAsync(
        string? filterOn = null, string? filterQuery = null, 
        string? sortBy = null, bool isAscending = true, 
        int pageNumber = 1, int pageSize = 1000)
    {
        /*return await _dbContext.Walks
            .Include(w => w.Region)
            .Include(w => w.Difficulty)
            .ToListAsync();*/
        var walks = _dbContext.Walks
            .Include("Region")
            .Include("Difficulty")
            .AsQueryable();

        //Filtering
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterOn) == false)
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(w => w.Name.Contains(filterQuery));
            }
        }
        
        //Sorting
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
            }
            else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
            }
        }
        
        //Pagination 
        var skippedResults = (pageNumber - 1) * pageSize;

        return await walks.Skip(skippedResults).Take(pageSize).ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Walks
            .Include("Difficulty")
            .Include("Region")
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var walkFromDb = await _dbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

        if (walkFromDb == null)
        {
            return null;
        }

        walkFromDb.Name = walk.Name;
        walkFromDb.WalkImageUrl = walk.WalkImageUrl;
        walkFromDb.LengthInKm = walk.LengthInKm;
        walkFromDb.Description = walk.Description;
        walkFromDb.DifficultyId = walk.DifficultyId;
        walkFromDb.RegionId = walk.RegionId;

        await _dbContext.SaveChangesAsync();
        return walkFromDb;
    }

    public async Task<Walk?> DeleteByIdAsync(Guid id)
    {
        var walkFromDb = await _dbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

        if (walkFromDb == null)
        {
            return null;
        }

        _dbContext.Walks.Remove(walkFromDb);
        await _dbContext.SaveChangesAsync();
        return walkFromDb;
    }
}