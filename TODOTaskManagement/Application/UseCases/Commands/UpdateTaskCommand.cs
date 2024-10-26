using Domain.Entities;
using MediatR;

namespace Application.UseCases.Commands
{
    public class UpdateTaskCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
