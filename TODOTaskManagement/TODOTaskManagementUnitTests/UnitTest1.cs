using Application.DTOs;
using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Application.UseCases.CommandHandlers;
using Application.UseCases.QueryHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Application.Utils;

namespace TODOTaskManagementUnitTests
{
    public class CreateTaskCommandValidatorTests
    {
        private readonly CreateTaskCommandValidator _validator;

        public CreateTaskCommandValidatorTests()
        {
            _validator = new CreateTaskCommandValidator();
        }

        [Fact]
        public void ShouldHaveError_WhenTitleIsEmpty()
        {
            // Arrange
            var command = new CreateTaskCommand { Title = "", Description = "Test Description" };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void ShouldHaveError_WhenTitleExceeds200Characters()
        {
            // Arrange
            var command = new CreateTaskCommand { Title = new string('x', 201), Description = "Test Description" };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void ShouldHaveError_WhenDueDateIsInPast()
        {
            // Arrange
            var command = new CreateTaskCommand 
            { 
                Title = "Test", 
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DueDate);
        }
    }

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

    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_ShouldBeValid()
        {
            // Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_TaskEntityToTaskDTO_ShouldMapCorrectly()
        {
            // Arrange
            var entity = new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Test Description",
                State = TaskState.Pending,
                Priority = TaskPriority.Medium,
                CreatedAt = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1)
            };

            // Act
            var dto = _mapper.Map<TaskDTO>(entity);

            // Assert
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.Title, dto.Title);
            Assert.Equal(entity.Description, dto.Description);
            Assert.Equal(entity.State, dto.State);
            Assert.Equal(entity.Priority, dto.Priority);
            Assert.Equal(entity.CreatedAt, dto.CreatedAt);
            Assert.Equal(entity.DueDate, dto.DueDate);
        }
    }
}