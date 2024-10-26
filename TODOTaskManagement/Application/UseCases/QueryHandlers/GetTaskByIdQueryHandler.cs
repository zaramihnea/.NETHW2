using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO>
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        public GetTaskByIdQueryHandler(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<TaskDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await repository.GetByIdAsync(request.Id);
            return mapper.Map<TaskDTO>(task);
        }
    }
}
