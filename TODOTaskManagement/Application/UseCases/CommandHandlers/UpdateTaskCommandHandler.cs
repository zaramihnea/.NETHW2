using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        public UpdateTaskCommandHandler(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = mapper.Map<TaskEntity>(request);
            await repository.UpdateAsync(task);
            return Unit.Value;
        }
    }
}
