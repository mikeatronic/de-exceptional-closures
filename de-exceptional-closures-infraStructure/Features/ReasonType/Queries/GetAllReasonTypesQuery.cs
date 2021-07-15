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

namespace de_exceptional_closures_infraStructure.Features.ReasonType.Queries
{
    public class GetAllReasonTypesQuery : IRequest<Result<List<ReasonTypeDto>>>
    {
    }

    public class GetAllReasonTypesHandler : AbstractBaseHandler<GetAllReasonTypesQuery, Result<List<ReasonTypeDto>>>
    {
        public GetAllReasonTypesHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<GetAllReasonTypesQuery> validator) : base(applicationDbContext, mapper, validator)
        {

        }

        public override async Task<Result<List<ReasonTypeDto>>> Handle(GetAllReasonTypesQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)

                return Result.Fail<List<ReasonTypeDto>>(validationResult.ToString());

            var reasons = ApplicationDbContext.ReasonType.AsNoTracking().ToList();

            var reasonsDtos = Mapper.Map<List<ReasonTypeDto>>(reasons);

            return Result.Ok(reasonsDtos);
        }
    }
}