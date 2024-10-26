using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDTO>>
    {
    }
}
