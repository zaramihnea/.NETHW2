using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace TODOTaskManagementUnitTests
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateTaskCommandHandler _handler;

        public UpdateTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateTaskCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTask()
        {
            // Arrange
            var command = new UpdateTaskCommand
            {
                Id = Guid.NewGuid(),
                Title = "Updated Title",
                Description = "Updated Description",
                State = TaskState.Completed,
                Priority = TaskPriority.High
            };

            var taskEntity = new TaskEntity
            {
                Id = command.Id,
                Title = command.Title,
                Description = command.Description,
                State = command.State,
                Priority = command.Priority
            };

            _mapperMock.Setup(m => m.Map<TaskEntity>(command)).Returns(taskEntity);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(taskEntity), Times.Once);
        }
    }
}