using de_exceptional_closures_infraStructure.Features.ClosureReason.Queries;
using FluentValidation;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Validation
{
    public class GetOverlappingClosuresQueryValidtor : AbstractValidator<GetOverlappingClosuresQuery>
    {
        public GetOverlappingClosuresQueryValidtor()
        {
            RuleFor(x => x.ClosureReasonDto).NotNull().WithMessage("ClosureReasonDto not set");
        }
    }
}