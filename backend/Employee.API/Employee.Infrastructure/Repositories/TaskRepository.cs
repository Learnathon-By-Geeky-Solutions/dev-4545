

using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infrastructure.Repositories
{
    public class TaskRepository(AppDbContext dbContext): ITaskRepository
    {
        public async Task<IEnumerable<TaskEntity>> GetAllTasks()
        {
            var tasks = await dbContext.Tasks.ToListAsync();
            return tasks;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(Guid Id)
        {
            var result= await dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == Id);
            return result;
        }
        public async Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity)
        {
            taskEntity.TaskId = Guid.NewGuid();
            await dbContext.Tasks.AddAsync(taskEntity);
            dbContext.SaveChanges();
            return taskEntity;
        }

        public async Task<TaskEntity> UpdateTask(Guid id, TaskEntity taskEntity)
        {
            var data = await dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (data != null)
            {
                data.DueDate=taskEntity.DueDate;
                data.AssignedDate=taskEntity.AssignedDate;
                data.Status=taskEntity.Status;
                data.AssignedBy=taskEntity.AssignedBy;
                data.EmployeeId=taskEntity.EmployeeId;
                data.Description=taskEntity.Description;
                data.FeatureId=taskEntity.FeatureId;

                await dbContext.SaveChangesAsync();
                return data;
            }
            return taskEntity;
        }

        public async Task<bool> DeleteTask(Guid id)
        {
            var data = await dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (data != null)
            {
                dbContext.Tasks.Remove(data);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }




    }

    
}
