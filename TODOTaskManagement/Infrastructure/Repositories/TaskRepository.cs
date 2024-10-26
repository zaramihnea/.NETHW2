using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext context;

        public TaskRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> AddAsync(TaskEntity task)
        {
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return task.Id;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEntity> GetByIdAsync(Guid id)
        {
            return await context.Tasks.FindAsync(id);
        }

        public Task UpdateAsync(TaskEntity task)
        {
            throw new NotImplementedException();
        }
    }
}
