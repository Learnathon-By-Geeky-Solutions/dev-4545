using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class FeatureRepository(AppDbContext dbContext) : IFeatureRepository
    {
        public async Task<FeatureEntity> AddFeatureAsync(FeatureEntity feature)
        {
            feature.FeatureId = Guid.NewGuid();
            await dbContext.Features.AddAsync(feature);
            dbContext.SaveChangesAsync(); 
            return feature; ;
        }

        public async Task<bool> DeleteFeature(Guid Id)
        {
            var feature= await dbContext.Features.FirstOrDefaultAsync(x=>x.FeatureId==Id);
            if (feature != null) { 
                dbContext.Features.Remove(feature);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<FeatureEntity>> GetAllFeatures()
        {
            var Feature = await dbContext.Features.ToListAsync();
            return Feature;
        }

        public async Task<FeatureEntity> GetFeatureById(Guid Id)
        {
            var feature = await dbContext.Features.FirstOrDefaultAsync(x=>x.FeatureId==Id);
            return feature;
        }

        public async Task<FeatureEntity> UpdateFeature(Guid Id, FeatureEntity feature)
        {
            var data = await dbContext.Features.FirstOrDefaultAsync(x => x.FeatureId == Id);
            if (data != null)
            {
                data.StartDate = feature.StartDate;
                data.EndDate = feature.EndDate;
               
                data.FeatureName = feature.FeatureName;
                data.Description = feature.Description;
                await dbContext.SaveChangesAsync();
                return data;

            }
            return feature;
        }
    }
}
