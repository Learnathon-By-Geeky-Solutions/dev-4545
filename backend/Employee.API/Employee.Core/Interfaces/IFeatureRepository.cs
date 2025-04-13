using Employee.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Interfaces
{
    public interface IFeatureRepository
    {
        Task<IEnumerable<FeatureEntity>> GetAllFeatures();
        Task<IEnumerable<FeatureEntity>> GetFeatureByEmployeeId(Guid EmployeeId);
        Task<FeatureEntity> AddFeatureAsync(FeatureEntity feature);
        Task<FeatureEntity> UpdateFeature(Guid Id, FeatureEntity feature);
        Task<bool> DeleteFeature(Guid Id);
    }
}
