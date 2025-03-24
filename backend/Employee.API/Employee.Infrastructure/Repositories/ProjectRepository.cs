using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class ProjectRepository(AppDbContext dbContext):IProjectRepository
    {
        public async Task<IEnumerable<ProjectEntity>> GetAllProjects()
        {
            var projects = await dbContext.Projects.ToListAsync();
            return projects;
        }
        public async Task<ProjectEntity> GetProjectById(Guid Id)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(x=>x.ProjectId == Id);
            return project;
        }
        public async Task<ProjectEntity> AddProjectAsync(ProjectEntity project)
        {
            project.ProjectId = Guid.NewGuid();
            await dbContext.Projects.AddAsync(project);
            dbContext.SaveChanges();
            return project;
        }
        public async Task<ProjectEntity>UpdateProject(Guid Id, ProjectEntity project)
        {
            var data= await dbContext.Projects.FirstOrDefaultAsync(x=>x.ProjectId == Id);
            if(data != null)
            {
                data.StartDate = project.StartDate;
                data.EndDate = project.EndDate;
                data.Client=project.Client;
                data.ProjectName = project.ProjectName;
                data.Description = project.Description;
                await dbContext.SaveChangesAsync();
                return data;

            }
            return project;
        }

        public async Task<bool> DeleteProject(Guid Id)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == Id);
            if (project != null)
            {
                dbContext.Projects.Remove(project);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
