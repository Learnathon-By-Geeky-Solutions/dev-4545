using Employee.Core.Entities;

namespace Employee.Core.Interfaces
{
    public interface ITasksRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllTasks();
        Task<IEnumerable<TaskEntity>> GetTaskByEmployeeIdAsync(Guid EmployeeId);
        Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity);
        Task<TaskEntity> UpdateTask(Guid id, TaskEntity taskEntity);
        Task<bool> DeleteTask(Guid id);

    }
}
