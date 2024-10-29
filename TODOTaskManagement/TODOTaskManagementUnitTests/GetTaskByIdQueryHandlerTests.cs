using Application.DTOs;
using Application.UseCases.Queries;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TODOTaskManagementUnitTests
{
    public class GetTaskByIdQueryHandlerTests
    {
        private readonly ITaskRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly GetTaskByIdQueryHandler _handler;

        public GetTaskByIdQueryHandlerTests()
        {
            _repositoryMock = Substitute.For<ITaskRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new GetTaskByIdQueryHandler(_repositoryMock, _mapperMock);
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

            var taskDto = new TaskDto
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description"
            };

            _repositoryMock.GetByIdAsync(taskId).Returns(Task.FromResult(taskEntity));
            _mapperMock.Map<TaskDto>(taskEntity).Returns(taskDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            await _repositoryMock.Received(1).GetByIdAsync(taskId);
        }
    }
}