// Application/Common/Authorization/OwnEmployeeRequirement.cs
using Microsoft.AspNetCore.Authorization;

namespace Employee.Application.Common.Authorization
{
    public class OwnEmployeeRequirement : IAuthorizationRequirement { }
}