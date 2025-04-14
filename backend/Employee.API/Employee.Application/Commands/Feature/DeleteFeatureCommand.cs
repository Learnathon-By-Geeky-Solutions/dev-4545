using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Feature
{
    public record DeleteFeatureCommand(Guid Id):IRequest<bool>;
    public class DeleteFeatureCommandHandler(IFeatureRepository featureRepository)
        : IRequestHandler<DeleteFeatureCommand, bool>
    {
        public async Task<bool> Handle(DeleteFeatureCommand request, CancellationToken cancellationToken)
        {
            return await featureRepository.DeleteFeature(request.Id);
        }
    }
}
