using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
using AutoMapper;
using Domain.Repositories;
using Moq;

namespace TODOTaskManagementUnitTests
{
    public class DeleteTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteTaskCommandHandler _handler;

        public DeleteTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteTaskCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new DeleteTaskCommand { Id = taskId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(taskId), Times.Once);
        }
    }
}