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
    public record UpdateFeatureCommand(Guid Id,FeatureEntity Feature):IRequest<FeatureEntity>;
    public class UpdateFeatureCommandHandler(IFeatureRepository featureRepository)
        : IRequestHandler<UpdateFeatureCommand, FeatureEntity>
    {
        public async Task<FeatureEntity> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
        {
            return await featureRepository.UpdateFeature(request.Id,request.Feature);
        }
    }
}
