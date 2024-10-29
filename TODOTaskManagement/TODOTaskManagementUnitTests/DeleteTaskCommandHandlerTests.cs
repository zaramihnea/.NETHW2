using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
using AutoMapper;
using Domain.Repositories;
using NSubstitute;

namespace TODOTaskManagementUnitTests
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly ITaskRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _repositoryMock = Substitute.For<ITaskRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new DeleteTaskCommandHandler(_repositoryMock, _mapperMock);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandleIsCalled_ThenTaskShouldBeDeleted()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new DeleteTaskCommand { Id = taskId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _repositoryMock.Received(1).DeleteAsync(taskId);
        }

        [Fact]
        public async Task GivenNonExistentTask_WhenHandleIsCalled_ThenShouldNotThrowException()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new DeleteTaskCommand { Id = taskId };

            _repositoryMock.DeleteAsync(taskId).Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Null(exception);
            await _repositoryMock.Received(1).DeleteAsync(taskId);
        }
    }
}