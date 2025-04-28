using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Salary
{
    public record GetSalariesByEmployeeIdQuery(Guid EmployeeId):IRequest<SalaryEntity?>;
    public class GetSalariesByEmployeeIdQueryHandler(ISalaryRepository salaryRepository )
        : IRequestHandler<GetSalariesByEmployeeIdQuery, SalaryEntity?>
    {
        public async Task<SalaryEntity?> Handle(GetSalariesByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            var result = await salaryRepository.GetSalaryByEmployeeId(request.EmployeeId);
            return result;
        }
    }
}
