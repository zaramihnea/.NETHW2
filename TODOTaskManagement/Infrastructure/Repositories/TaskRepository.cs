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
            var toRemove = await context.Tasks.FindAsync(id);
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

        public async Task UpdateAsync(TaskEntity newTask)
        {
            var modifiedTask = await context.Tasks.FindAsync(newTask.Id);
            if (modifiedTask != null)
            {
                modifiedTask.Title = newTask.Title;
                modifiedTask.Description = newTask.Description;
                modifiedTask.Priority = newTask.Priority;
                modifiedTask.State = newTask.State;
                modifiedTask.DueDate = newTask.DueDate;
                modifiedTask.UpdatedAt = DateTime.Now;
            }
            context.Tasks.Entry(modifiedTask).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
