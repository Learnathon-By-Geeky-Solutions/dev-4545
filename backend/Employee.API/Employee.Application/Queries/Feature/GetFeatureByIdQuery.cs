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
    public record GetFeatureByIdQuery(Guid EmployeeId) : IRequest<IEnumerable<FeatureEntity>>;

    public class GetFeatureByIdQueryHandler(IFeatureRepository featureRepository)
        : IRequestHandler<GetFeatureByIdQuery, IEnumerable<FeatureEntity>>
    {
        public async Task<IEnumerable<FeatureEntity>> Handle(GetFeatureByIdQuery request, CancellationToken cancellationToken)
        {
            return await featureRepository.GetFeatureByEmployeeId(request.EmployeeId);
        }
    }
}
