using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Queries.Performance
{
    public record GetAllPerformancesQuery : IRequest<IEnumerable<PerformanceEntity>>;

    public class GetAllFeaturesQueryHandler(IPerformanceRepository performanceRepository)
        : IRequestHandler<GetAllPerformancesQuery, IEnumerable<PerformanceEntity>>
    {
        public async Task<IEnumerable<PerformanceEntity>> Handle(GetAllPerformancesQuery request, CancellationToken cancellationToken)
        {
            return await performanceRepository.GetAllPerformances();
        }
    }
}
