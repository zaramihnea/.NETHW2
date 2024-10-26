using Application.UseCases.Commands;
using AutoMapper;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.UseCases.CommandHandlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        public DeleteTaskCommandHandler(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            DeleteTaskCommandValidator validationRules = new DeleteTaskCommandValidator();
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
            await repository.DeleteAsync(request.Id);
        }
    }
}
