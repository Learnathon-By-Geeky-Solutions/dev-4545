using System.Reflection.Metadata.Ecma335;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Employee
{
    public record GetEmployeeQuery : IRequest<IEnumerable<EmployeeEntity>>;

    public class GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<GetEmployeeQuery, IEnumerable<EmployeeEntity>>
    {
        public async Task<IEnumerable<EmployeeEntity>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            return await employeeRepository.GetEmployees();
        }
    }
}
