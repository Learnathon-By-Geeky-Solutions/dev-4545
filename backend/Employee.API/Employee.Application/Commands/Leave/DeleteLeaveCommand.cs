using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Leave
{
    public record DeleteLeaveByEmpIdCommand(Guid EmployeeId) : IRequest<bool>;
    public class DeleteLeaveCommandHandler(ILeaveRepository LeaveRepository)
        : IRequestHandler<DeleteLeaveByEmpIdCommand, bool>
    {
        public async Task<bool> Handle(DeleteLeaveByEmpIdCommand request, CancellationToken cancellationToken)
        {
            var result = await LeaveRepository.DeleteLeaveByEmployeeId(request.EmployeeId);
            return result;
        }
    }
}
