using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Feature
{
    public record AddFeatureCommand(FeatureEntity Feature):IRequest<FeatureEntity>;
    public class AddFeatureCommandHandler(IFeatureRepository featureRepository)
        : IRequestHandler<AddFeatureCommand, FeatureEntity>
    {
        public async Task<FeatureEntity> Handle(AddFeatureCommand request, CancellationToken cancellationToken)
        {
            return await featureRepository.AddFeatureAsync(request.Feature);
        }
    }
}
