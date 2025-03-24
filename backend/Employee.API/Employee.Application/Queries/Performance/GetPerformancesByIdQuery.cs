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
    public record GetPerformancesByIdQuery(Guid Id) : IRequest<PerformanceEntity>;

    public class GetPerformanceByIdQueryHandler(IPerformanceRepository performanceRepository)
        : IRequestHandler<GetPerformancesByIdQuery, PerformanceEntity>
    {
        public async Task<PerformanceEntity> Handle(GetPerformancesByIdQuery request, CancellationToken cancellationToken)
        {
            return await performanceRepository.GetPerformancesById(request.Id);
        }
    }
}
