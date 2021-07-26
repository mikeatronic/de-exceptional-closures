using de_exceptional_closures_infraStructure.Features.ClosureReason.Commands;
using FluentValidation;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Validation
{
    public class UpdateClosureReasonCommandValidator : AbstractValidator<UpdateClosureReasonCommand>
    {
        public UpdateClosureReasonCommandValidator()
        {
            RuleFor(d => d.ClosureReasonDto).NotNull().WithMessage("Closure reason is not set");
        }
    }
}