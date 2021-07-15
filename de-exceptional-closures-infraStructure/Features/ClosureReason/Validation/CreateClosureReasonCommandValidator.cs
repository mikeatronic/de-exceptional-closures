using de_exceptional_closures_infraStructure.Features.ClosureReason.Commands;
using FluentValidation;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Validation
{
    public class CreateClosureReasonCommandValidator : AbstractValidator<CreateClosureReasonCommand>
    {
        public CreateClosureReasonCommandValidator()
        {
            RuleFor(x => x.ClosureReasonDto).NotNull().WithMessage("ClosureReasonDto not set");
        }
    }
}