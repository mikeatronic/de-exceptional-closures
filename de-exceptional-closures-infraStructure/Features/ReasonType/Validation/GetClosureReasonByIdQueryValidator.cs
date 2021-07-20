using de_exceptional_closures_infraStructure.Features.ClosureReason.Queries;
using FluentValidation;

namespace de_exceptional_closures_infraStructure.Features.ReasonType.Validation
{
    public class GetClosureReasonByIdQueryValidator : AbstractValidator<GetClosureReasonByIdQuery>
    {
        public GetClosureReasonByIdQueryValidator()
        {
            RuleFor(q => q.Id).GreaterThan(0).WithMessage("Id not set");
        }
    }
}