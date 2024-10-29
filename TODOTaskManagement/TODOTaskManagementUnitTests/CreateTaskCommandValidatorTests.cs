using Application.UseCases.Commands;
using Domain.Entities;
using FluentValidation.TestHelper;

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
        public void GivenValidCommand_WhenValidated_ThenShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Test Description",
                State = TaskState.Pending,
                Priority = TaskPriority.Medium
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void GivenInvalidCommand_WhenValidated_ThenShouldHaveValidationErrors()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "", // Invalid title
                Description = "Test Description",
                State = TaskState.Pending,
                Priority = TaskPriority.Medium
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }
    }
}