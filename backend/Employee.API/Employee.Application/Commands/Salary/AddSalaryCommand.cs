using System.Collections.Specialized;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using Microsoft.VisualBasic;

namespace Employee.Application.Commands.Salary
{
    public record AddSalaryCommand(SalaryEntity salary) : IRequest<SalaryEntity>;

    public class AddSalaryCommandHandler(ISalaryRepository salaryRepository)
        : IRequestHandler<AddSalaryCommand, SalaryEntity>
    {
        public async Task<SalaryEntity> Handle(AddSalaryCommand request, CancellationToken cancellationToken)
        {
            return await salaryRepository.AddSalary(request.salary);
        }
    }
}


