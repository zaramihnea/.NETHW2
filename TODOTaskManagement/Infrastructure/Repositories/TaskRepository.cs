using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task DeleteAsync(Guid id)
        {
            var toRemove = context.Tasks.FirstOrDefault(x => x.Id == id);
            if (toRemove != null)
            {
                context.Tasks.Remove(toRemove);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            var tasks = await context.Tasks.ToListAsync();
            return tasks;
        }

        public async Task<TaskEntity> GetByIdAsync(Guid id)
        {
            return await context.Tasks.FindAsync(id);
        }

        public async Task UpdateAsync(TaskEntity task)
        {
            context.Entry(task).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
