using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepositoryImpl : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        //constructor of the class
        public RegionRepositoryImpl(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            //throw new NotImplementedException();

            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbContext.Regions.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            //throw new NotImplementedException();
            // if region is null , return null

            if (region == null)
            {
                return region;
            }
            // if not null, delete the region . 
            nZWalksDbContext.Regions.Remove(region);

            await nZWalksDbContext.SaveChangesAsync();
            return region;

        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var exsistingRegion = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            // if region is null , return null 
            if (exsistingRegion == null)
                return null;
            // if not null , update the region with the values from the client .
            exsistingRegion.Code = region.Code;
            exsistingRegion.Name = region.Name;
            exsistingRegion.Area = region.Area;
            exsistingRegion.Lat = region.Lat;
            exsistingRegion.Long = region.Long;
            exsistingRegion.Population = region.Population;

            await nZWalksDbContext.SaveChangesAsync();

            return exsistingRegion;
        }
    }
}
