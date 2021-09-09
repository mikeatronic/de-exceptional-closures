using AutoMapper;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Features.Common;
using de_exceptional_closures_Infrastructure.Data;
using dss_common.Functional;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Queries
{
    public class GetOverlappingClosuresQuery : IRequest<Result<bool>>
    {
        public ClosureReasonDto ClosureReasonDto { get; set; }
    }

    public class GetOverlappingClosuresQueryHandler : AbstractBaseHandler<GetOverlappingClosuresQuery, Result<bool>>
    {
        public GetOverlappingClosuresQueryHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<GetOverlappingClosuresQuery> validator) : base(applicationDbContext, mapper, validator)
        {

        }

        public override async Task<Result<bool>> Handle(GetOverlappingClosuresQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<bool>(validationResult.ToString());


            var getAllClosuresDatesForSchool = ApplicationDbContext.ClosureReason.AsNoTracking().Where(p => p.Srn == request.ClosureReasonDto.Srn).OrderByDescending(i => i.DateFrom);

            foreach (var item in getAllClosuresDatesForSchool)
            {
                // bool overlap = a.start < b.end && b.start < a.end;


                bool overlap = request.ClosureReasonDto.DateFrom < item.DateTo && item.DateTo < request.ClosureReasonDto.DateTo;

                if (overlap)
                {
                    return Result.Ok(true);
                }
            }

            return Result.Ok(false);
        }
    }
}