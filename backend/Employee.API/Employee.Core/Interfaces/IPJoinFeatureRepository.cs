using Employee.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Interfaces
{
    public interface IPJoinFeatureRepository
    {
        Task<IEnumerable<PJoinFeatureEntity>> GetAllFeatures();
        Task<PJoinFeatureEntity> GetFeatureById(Guid Id);
        Task<PJoinFeatureEntity> AddFeatureAsync(PJoinFeatureEntity join);
        Task<PJoinFeatureEntity>UpdateFeature(Guid Id, PJoinFeatureEntity join);
        Task<bool> DeleteFeature(Guid Id);
    }
}
