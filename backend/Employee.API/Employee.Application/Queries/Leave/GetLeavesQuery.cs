using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Leave
{
    public record GetLeaveQuery() : IRequest<IEnumerable<LeaveEntity>>;
    public class GetLeaveQueryHanlder(ILeaveRepository LeaveRepository)
        : IRequestHandler<GetLeaveQuery, IEnumerable<LeaveEntity>>
    {

        async Task<IEnumerable<LeaveEntity>> IRequestHandler<GetLeaveQuery, IEnumerable<LeaveEntity>>.Handle(GetLeaveQuery request, CancellationToken cancellationToken)
        {
            var result = await LeaveRepository.GetLeaves();
            return result;
        }
    }
}
