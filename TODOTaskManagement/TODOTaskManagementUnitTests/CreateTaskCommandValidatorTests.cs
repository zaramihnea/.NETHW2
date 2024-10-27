using Application.UseCases.Commands;
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
}