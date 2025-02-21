using Employee.Core.Interfaces;
using Management.Core.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Login
{
    public record LoginCommand(AuthenticationRequest response): IRequest<AuthenticationResponse>;
    public class LoginCommandHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.Authenticate(request.response);
        }
    }
}
