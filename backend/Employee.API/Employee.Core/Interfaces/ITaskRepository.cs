using Employee.Core.Entities;

namespace Employee.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllTasks();
        Task<TaskEntity> GetTaskByIdAsync(Guid Id);
        Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity);
        Task<TaskEntity> UpdateTask(Guid id, TaskEntity taskEntity);
        Task<bool> DeleteTask(Guid id);

    }
}
