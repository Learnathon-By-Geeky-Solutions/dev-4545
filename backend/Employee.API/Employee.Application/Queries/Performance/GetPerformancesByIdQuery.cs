using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Performance
{
    public record GetPerformancesByIdQuery(Guid EmployeeId) : IRequest<PerformanceEntity?>;

    public class GetPerformanceByIdQueryHandler(IPerformanceRepository performanceRepository)
        : IRequestHandler<GetPerformancesByIdQuery, PerformanceEntity?>
    {
        public async Task<PerformanceEntity?> Handle(GetPerformancesByIdQuery request, CancellationToken cancellationToken)
        {
            return await performanceRepository.GetPerformancesByEmployeeId(request.EmployeeId);
        }
    }
}
