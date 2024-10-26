using MediatR;

namespace Application.UseCases.Commands
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
