using Application.UseCases.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        public UpdateTaskCommandHandler(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            UpdateTaskCommandValidator validationRules = new UpdateTaskCommandValidator();
            var validator = validationRules.Validate(request);
            if (!validator.IsValid)
            {
                var errorsResult = new List<string>();
                foreach (var error in validator.Errors)
                {
                    errorsResult.Add(error.ErrorMessage);
                }
                throw new ValidationException(errorsResult.ToString());
            }
            var task = mapper.Map<TaskEntity>(request);
            return repository.UpdateAsync(task);
        }
    }
}
