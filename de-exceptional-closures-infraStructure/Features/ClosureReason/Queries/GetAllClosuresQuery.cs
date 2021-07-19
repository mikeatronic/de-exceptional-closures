using AutoMapper;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Features.Common;
using de_exceptional_closures_Infrastructure.Data;
using dss_common.Functional;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Queries
{
    public class GetAllClosuresQuery : IRequest<Result<List<ClosureReasonDto>>>
    {
    }

    public class GetAllClosuresQueryHandler : AbstractBaseHandler<GetAllClosuresQuery, Result<List<ClosureReasonDto>>>
    {
        public GetAllClosuresQueryHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<GetAllClosuresQuery> validator) : base(applicationDbContext, mapper, validator)
        {

        }
        public override async Task<Result<List<ClosureReasonDto>>> Handle(GetAllClosuresQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<List<ClosureReasonDto>>(validationResult.ToString());

            var applications = ApplicationDbContext.ClosureReason
                .AsNoTracking()
                .Include(c => c.ReasonType)
                .Include(c => c.ApprovalType)
                .ToList();

            var applicationHeaders = Mapper.Map<List<ClosureReasonDto>>(applications);

            return Result.Ok(applicationHeaders);
        }
    }
}