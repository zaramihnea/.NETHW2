using Domain.Entities;

namespace Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task<TaskEntity> GetByIdAsync(Guid id);
        Task<Guid> AddAsync (TaskEntity task);
        Task UpdateAsync(TaskEntity task);
        Task DeleteAsync(Guid id);
    }
}
