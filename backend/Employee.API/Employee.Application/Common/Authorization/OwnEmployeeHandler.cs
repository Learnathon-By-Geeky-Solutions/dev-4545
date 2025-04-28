using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Employee.Application.Common.Authorization
{
    public class OwnEmployeeHandler
        : AuthorizationHandler<OwnEmployeeRequirement, Guid>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnEmployeeRequirement requirement,
            Guid targetEmployeeId)
        {
            // Admins always succeed
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Otherwise, must match own employeeId claim
            var claim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var myId) && myId == targetEmployeeId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}