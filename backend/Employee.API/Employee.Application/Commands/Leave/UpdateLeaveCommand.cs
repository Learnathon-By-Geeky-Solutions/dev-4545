using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Leave
{
    public record UpdateLeaveCommand(Guid EmployeeId, LeaveEntity LeaveEntity) : IRequest<LeaveEntity>;
    public class UpdateLeaveCommandHandler(ILeaveRepository LeaveRepository)
        : IRequestHandler<UpdateLeaveCommand, LeaveEntity>
    {
        public async Task<LeaveEntity> Handle(UpdateLeaveCommand request, CancellationToken cancellationToken)
        {
            var result = await LeaveRepository.UpdateLeave(request.EmployeeId, request.LeaveEntity);
            return result;
        }
    }
}
