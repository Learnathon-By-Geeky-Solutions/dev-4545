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
    public record LoginCommand(AuthenticationRequest response): IRequest<AuthenticationResponse>;// we want AuthenticationResponse as Return 
    public class LoginCommandHandler(IEmployeeRepository employeeRepository)//Repository
        : IRequestHandler<LoginCommand, AuthenticationResponse>//AuthenticationResponse will return us
    {
        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await employeeRepository.Authenticate(request.response);
        }
    }
}
