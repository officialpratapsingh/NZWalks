using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepositoryImpl : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        // constructor injected with db context 
        public WalkDifficultyRepositoryImpl(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        //add method 
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            // find the record to be deleted using firstORdefault async, if null , return null
            var walkDifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDifficulty == null)
            {
                return null;
            }
            // remove from the db using nzwalks 
            nZWalksDbContext.WalkDifficulty.Remove(walkDifficulty);
            // save changes in the db 
            await nZWalksDbContext.SaveChangesAsync();
            // return the deleted rec to the client 
            return walkDifficulty;
        }

        // get all method 
        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }
        // get byId mehtod
        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            // use dbcontext & use firstordefualt to find the obj 
            var walkDifficulty = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDifficulty != null)
            {
                return walkDifficulty;
            }

            return null;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var walkD = await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if (walkD != null)
            {
                walkD.Code = walkDifficulty.Code;
                await nZWalksDbContext.SaveChangesAsync();
                return walkD;
            }

            return null;

        }
    }
}
