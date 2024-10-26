using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetTaskByIdQuery : IRequest<TaskDTO>
    {
        public Guid Id { get; set; }
    }
}
