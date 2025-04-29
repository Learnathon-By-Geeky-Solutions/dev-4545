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
    public record GetFeatureByEmployeeIdQuery(Guid EmployeeId) : IRequest<IEnumerable<FeatureEntity>>;

    public class GetFeatureByEmployeeIdQueryHandler(IFeatureRepository featureRepository)
        : IRequestHandler<GetFeatureByEmployeeIdQuery, IEnumerable<FeatureEntity>>
    {
        public async Task<IEnumerable<FeatureEntity>> Handle(GetFeatureByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            return await featureRepository.GetFeatureByEmployeeId(request.EmployeeId);
        }
    }
}
