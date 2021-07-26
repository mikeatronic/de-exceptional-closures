using AutoMapper;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Features.Common;
using de_exceptional_closures_Infrastructure.Data;
using dss_common.Extensions;
using dss_common.Functional;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Queries
{
    public class GetClosureReasonByIdQuery : IRequest<Result<ClosureReasonDto>>
    {
        public int Id { get; set; }
    }

    public class GetClosureReasonByIdQueryHandler : AbstractBaseHandler<GetClosureReasonByIdQuery, Result<ClosureReasonDto>>
    {
        public GetClosureReasonByIdQueryHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<GetClosureReasonByIdQuery> validator) : base(applicationDbContext, mapper, validator)
        {

        }

        public override async Task<Result<ClosureReasonDto>> Handle(GetClosureReasonByIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<ClosureReasonDto>(validationResult.ToString());

            var maybeClosure = (await ApplicationDbContext.ClosureReason.Include(c => c.ReasonType)
                .Include(c => c.ApprovalType)
                .Include(c => c.RejectionReason)
                .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken)).ToMaybe();

            if (!maybeClosure.HasValue)
                return Result.Fail<ClosureReasonDto>("Closure reason not found");

            var closureReasonDto = Mapper.Map<ClosureReasonDto>(maybeClosure.Value);

            return Result.Ok(closureReasonDto);
        }
    }
}