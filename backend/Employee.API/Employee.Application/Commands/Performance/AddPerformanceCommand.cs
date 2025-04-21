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
    public record AddPerformanceCommand(PerformanceEntity performance) : IRequest<PerformanceEntity>;
    public class AddPerformanceCommandHandler(IPerformanceRepository performanceRepository)
        : IRequestHandler<AddPerformanceCommand, PerformanceEntity>
    {
        public async Task<PerformanceEntity> Handle(AddPerformanceCommand request, CancellationToken cancellationToken)
        {
            return await performanceRepository.AddPerformanceAsync(request.performance);
        }
    }
}
