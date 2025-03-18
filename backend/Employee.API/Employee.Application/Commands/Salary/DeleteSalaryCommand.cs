using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Salary
{
    public record DeleteSalaryByEmpIdCommand(Guid EmployeeId): IRequest<bool>;
    public class DeleteSalaryCommandHandler(ISalaryRepository salaryRepository)
        : IRequestHandler<DeleteSalaryByEmpIdCommand, bool>
    {
        public async Task<bool> Handle(DeleteSalaryByEmpIdCommand request, CancellationToken cancellationToken)
        {
            var result= await salaryRepository.DeleteSalaryByEmployeeId(request.EmployeeId);
            return result;
        }
    }
}
