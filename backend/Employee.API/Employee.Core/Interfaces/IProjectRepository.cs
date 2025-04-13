using Employee.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Core.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectEntity>> GetAllProjects();
        Task<IEnumerable<ProjectEntity>> GetProjectByEmployeeId(Guid Id);
        Task<ProjectEntity> AddProjectAsync(ProjectEntity project);
        Task<ProjectEntity> UpdateProject(Guid Id, ProjectEntity project);
        Task<bool> DeleteProject(Guid Id);

    }
}
