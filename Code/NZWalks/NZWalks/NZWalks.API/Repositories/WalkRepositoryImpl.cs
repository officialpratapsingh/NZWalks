using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepositoryImpl : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public WalkRepositoryImpl(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // passing new Id , as the ID field cannot be entered by the user .
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);

            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            // use dbcontext to find the record to be deleted 
            var walk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            // if walk is not null , use the .remove fxn to delete the walk 
            if (walk != null)
            {
                nZWalksDbContext.Walks.Remove(walk);
                await nZWalksDbContext.SaveChangesAsync();
                return walk;
               
            }
            // if null, return null 
            return null;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
          return  
                await nZWalksDbContext.
                Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public  Task<Walk> GetAsync(Guid id)
        {
           return
                nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var exsistingWalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (exsistingWalk != null)
            {
                exsistingWalk.Name = walk.Name;
                exsistingWalk.Length = walk.Length;
                exsistingWalk.RegionId = walk.RegionId;
                exsistingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                await nZWalksDbContext.SaveChangesAsync();

                return exsistingWalk;
            }
            else 
                return null;
            
            




        }
    }
}
