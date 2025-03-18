using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Salary
{
    public record GetSalaryQuery():IRequest<IEnumerable<SalaryEntity>> ;
    public class GetSalayQueryHandler(ISalaryRepository salaryRepository)
        : IRequestHandler<GetSalaryQuery, IEnumerable<SalaryEntity>>
    {

        async Task<IEnumerable<SalaryEntity>> IRequestHandler<GetSalaryQuery, IEnumerable<SalaryEntity>>.Handle(GetSalaryQuery request, CancellationToken cancellationToken)
        {
            var result = await salaryRepository.GetSalaries();
            return result;
        }
    }
}
