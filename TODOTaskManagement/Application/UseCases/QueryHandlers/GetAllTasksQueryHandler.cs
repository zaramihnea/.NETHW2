using Application.DTOs;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDTO>>
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        public GetAllTasksQueryHandler(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await repository.GetAllAsync();
            return mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }
    }
}