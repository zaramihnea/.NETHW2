using MediatR;

namespace Application.UseCases.Commands
{
    public class DeleteTaskCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
