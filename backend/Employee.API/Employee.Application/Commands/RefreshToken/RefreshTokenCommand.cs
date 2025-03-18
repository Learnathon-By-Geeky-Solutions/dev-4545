using Employee.Application.Commands.Login;
using Employee.Core.Interfaces;
using Management.Core.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token) : IRequest<string>;//IReques <string> :what we want to get from function
    public class RefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository)
        : IRequestHandler<RefreshTokenCommand, string>//here string means what we want to get
    {
        public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await refreshTokenRepository.GetAccessToken(request.Token);
        }
    }
}
