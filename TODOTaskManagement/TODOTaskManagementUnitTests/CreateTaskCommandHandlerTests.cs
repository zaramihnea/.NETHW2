using Application.DTOs;
using Application.UseCases.Commands;
using Application.UseCases.CommandHandlers;
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
    public class CreateTaskCommandHandlerTests
    {
        private readonly ITaskRepository _repositoryMock;
        private readonly IMapper _mapperMock;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _repositoryMock = Substitute.For<ITaskRepository>();
            _mapperMock = Substitute.For<IMapper>();
            _handler = new CreateTaskCommandHandler(_repositoryMock, _mapperMock);
        }

        [Fact]
        public async Task GivenValidCommand_WhenHandleIsCalled_ThenTaskShouldBeCreatedAndReturnId()
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

            _mapperMock.Map<TaskEntity>(command).Returns(taskEntity);
            _repositoryMock.AddAsync(taskEntity).Returns(Task.FromResult(taskEntity.Id));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(taskEntity.Id, result);
            await _repositoryMock.Received(1).AddAsync(taskEntity);
        }

        [Fact]
        public async Task GivenInvalidMapping_WhenHandleIsCalled_ThenShouldThrowException()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Test Description",
                State = TaskState.Pending,
                Priority = TaskPriority.Medium
            };

            _mapperMock.Map<TaskEntity>(command).Returns((TaskEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}