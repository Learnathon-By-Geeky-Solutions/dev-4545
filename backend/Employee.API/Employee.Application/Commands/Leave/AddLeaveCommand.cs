using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Leave
{
    public record AddLeaveCommand(LeaveEntity Leave) : IRequest<LeaveEntity>;

    public class AddLeaveCommandHandler(ILeaveRepository LeaveRepository)
        : IRequestHandler<AddLeaveCommand, LeaveEntity>
    {
        public async Task<LeaveEntity> Handle(AddLeaveCommand request, CancellationToken cancellationToken)
        {
            return await LeaveRepository.AddLeave(request.Leave);
        }
    }
}


