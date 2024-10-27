using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace TODOTaskManagementUnitTests
{
    public class GetTaskByIdQueryHandlerTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTaskByIdQueryHandler _handler;

        public GetTaskByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetTaskByIdQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTaskDTO_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var query = new GetTaskByIdQuery { Id = taskId };
            
            var taskEntity = new TaskEntity 
            { 
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description"
            };
            
            var taskDto = new TaskDTO 
            { 
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(taskEntity);
            _mapperMock.Setup(m => m.Map<TaskDTO>(taskEntity)).Returns(taskDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            _repositoryMock.Verify(r => r.GetByIdAsync(taskId), Times.Once);
        }
    }
}