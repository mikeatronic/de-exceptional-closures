using de_exceptional_closures_infraStructure.Features.ClosureReason.Queries;
using FluentValidation;

namespace de_exceptional_closures_infraStructure.Features.ReasonType.Validation
{
    public class GetAllClosuresQueryValidator : AbstractValidator<GetAllClosuresQuery>
    {
        public GetAllClosuresQueryValidator()
        {
        }
    }
}