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

namespace de_exceptional_closures_infraStructure.Features.AutoApprovalList.Queries
{
    public class GetAllApprovalListQuery : IRequest<Result<List<AutoApprovalListDto>>>
    {
    }

    public class GetAllApprovalListQueryHandler : AbstractBaseHandler<GetAllApprovalListQuery, Result<List<AutoApprovalListDto>>>
    {
        public GetAllApprovalListQueryHandler(ApplicationDbContext schoolPortalContext, IMapper mapper, IValidator<GetAllApprovalListQuery> validator) : base(schoolPortalContext, mapper, validator)
        {

        }

        public override async Task<Result<List<AutoApprovalListDto>>> Handle(GetAllApprovalListQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)

                return Result.Fail<List<AutoApprovalListDto>>(validationResult.ToString());

            var reasons = ApplicationDbContext.AutoApprovalList.AsNoTracking().ToList();

            var reasonsDtos = Mapper.Map<List<AutoApprovalListDto>>(reasons);

            return Result.Ok(reasonsDtos);
        }
    }
}