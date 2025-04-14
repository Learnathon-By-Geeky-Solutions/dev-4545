using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Performance
{
    public record DeletePerformanceCommand(Guid Id) : IRequest<bool>;
    public class DeletePerformanceCommandHandler(IPerformanceRepository performanceRepository)
        : IRequestHandler<DeletePerformanceCommand, bool>
    {
        public async Task<bool> Handle(DeletePerformanceCommand request, CancellationToken cancellationToken)
        {
           return await performanceRepository.DeletePerformance(request.Id);    
        }
    }
}
