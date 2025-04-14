using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Leave
{
    public record GetLeavesByEmployeeIdQuery(Guid EmployeeId) : IRequest<LeaveEntity>;
    public class GetLeavesByEmployeeIdQueryHandler(ILeaveRepository LeaveRepository)
        : IRequestHandler<GetLeavesByEmployeeIdQuery, LeaveEntity>
    {
        public async Task<LeaveEntity> Handle(GetLeavesByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            var result = await LeaveRepository.GetLeaveByEmployeeId(request.EmployeeId);
            return result;
        }
    }
}
