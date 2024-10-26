using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TODOTaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator mediator;

        public TasksController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDTO>> GetById(Guid id)
        {
            return await mediator.Send(new GetTaskByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTask(CreateTaskCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction("GetById", new { Id = id }, id);
        }
        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteById(Guid id)
        {
            var query = new DeleteTaskCommand { Id = id };
            await mediator.Send(query);
            return NoContent();
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update(UpdateTaskCommand update)
        {
            await mediator.Send(update);
            return NoContent();
        }
        [HttpGet("All")]
        public async Task<IEnumerable<TaskDTO>> GetAll()
        {
            var query = new GetAllTasksQuery();
            IEnumerable<TaskDTO> enumerable = await mediator.Send(query);
            return enumerable;
        }
    }
}
