using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Salary
{
    public record UpdateSalaryCommand(Guid EmployeeId,SalaryEntity SalaryEntity): IRequest<SalaryEntity>;
    public class UpdateSalaryCommandHandler(ISalaryRepository salaryRepository)
        : IRequestHandler<UpdateSalaryCommand, SalaryEntity>
    {
        public async Task<SalaryEntity> Handle(UpdateSalaryCommand request, CancellationToken cancellationToken)
        {
            var result = await salaryRepository.UpdateSalary(request.EmployeeId, request.SalaryEntity);
            return result;
        }
    }
}
