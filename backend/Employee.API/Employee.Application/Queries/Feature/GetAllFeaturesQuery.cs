using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Queries.Feature
{
    public record GetAllFeaturesQuery : IRequest<IEnumerable<FeatureEntity>>;

    public class GetAllFeaturesQueryHandler(IFeatureRepository featureRepository)
        : IRequestHandler<GetAllFeaturesQuery, IEnumerable<FeatureEntity>>
    {
        public async Task<IEnumerable<FeatureEntity>> Handle(GetAllFeaturesQuery request, CancellationToken cancellationToken)
        {
            return await featureRepository.GetAllFeatures();
        }
    }
}
