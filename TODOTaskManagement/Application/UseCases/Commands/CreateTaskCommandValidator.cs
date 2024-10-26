using FluentValidation;

namespace Application.UseCases.Commands
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.State).NotEmpty().IsInEnum();
            RuleFor(x => x.Priority).NotEmpty().IsInEnum();
            RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTime.Now).When(x => x.DueDate.HasValue);
        }
    }
}
