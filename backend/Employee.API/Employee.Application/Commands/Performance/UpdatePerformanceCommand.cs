using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Performance
{
    public record UpdatePerformanceCommand(Guid Id, PerformanceEntity Performance) : IRequest<PerformanceEntity>;
    public class UpdatePerformanceCommandHandler(IPerformanceRepository performanceRepository)
        : IRequestHandler<UpdatePerformanceCommand, PerformanceEntity>
    {
        public async Task<PerformanceEntity> Handle(UpdatePerformanceCommand request, CancellationToken cancellationToken)
        {
            return await performanceRepository.UpdatePerformance(request.Id, request.Performance);
        }
    }
}
