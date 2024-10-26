using FluentValidation;

namespace Application.UseCases.Commands
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.State).NotEmpty().IsInEnum();
            RuleFor(x => x.Priority).NotEmpty().IsInEnum();
            RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTime.Now).When(x => x.DueDate.HasValue);
            RuleFor(x => x.Id).NotEmpty().Must(BeAValidGuid).WithMessage("Must be a valid guid");
        }
        private bool BeAValidGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
