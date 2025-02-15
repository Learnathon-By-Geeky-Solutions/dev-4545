using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Employee
{
    public record UpdateEmployeeCommand(Guid Id, EmployeeEntity employee) : IRequest<EmployeeEntity>;
    public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<UpdateEmployeeCommand, EmployeeEntity>
    {
        public async Task<EmployeeEntity> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.UpdateEmployee(request.Id, request.employee);
        }
    }
}
