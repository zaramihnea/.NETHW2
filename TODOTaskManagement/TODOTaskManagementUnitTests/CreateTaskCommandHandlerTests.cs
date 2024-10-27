using Application.DTOs;
using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace TODOTaskManagementUnitTests
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateTaskCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateTask_AndReturnId()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Test Description",
                State = TaskState.Pending,
                Priority = TaskPriority.Medium
            };

            var taskEntity = new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Description = command.Description,
                State = command.State,
                Priority = command.Priority
            };

            _mapperMock.Setup(m => m.Map<TaskEntity>(command)).Returns(taskEntity);
            _repositoryMock.Setup(r => r.AddAsync(taskEntity)).ReturnsAsync(taskEntity.Id);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(taskEntity.Id, result);
            _repositoryMock.Verify(r => r.AddAsync(taskEntity), Times.Once);
        }
    }
}