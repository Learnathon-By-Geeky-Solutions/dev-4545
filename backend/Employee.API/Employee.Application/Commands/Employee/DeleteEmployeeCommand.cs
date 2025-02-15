using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Employee
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<bool>;
    public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.DeleteEmployee(request.Id);

        }
    }
}
