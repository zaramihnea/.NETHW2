using Application.UseCases.Commands;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        public DeleteTaskCommandHandler(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
