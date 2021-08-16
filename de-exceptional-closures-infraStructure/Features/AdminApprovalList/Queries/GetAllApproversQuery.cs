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

namespace de_exceptional_closures_infraStructure.Features.AdminApprovalList.Queries
{
    public class GetAllApproversQuery : IRequest<Result<List<AdminApprovalListDto>>>
    {

    }

    public class GetAllApproversQueryHandler : AbstractBaseHandler<GetAllApproversQuery, Result<List<AdminApprovalListDto>>>
    {
        public GetAllApproversQueryHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<GetAllApproversQuery> validator) : base(applicationDbContext, mapper, validator)
        {

        }

        public override async Task<Result<List<AdminApprovalListDto>>> Handle(GetAllApproversQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)

                return Result.Fail<List<AdminApprovalListDto>>(validationResult.ToString());

            var reasons = ApplicationDbContext.AdminApprovalList.AsNoTracking().ToList();

            var reasonsDtos = Mapper.Map<List<AdminApprovalListDto>>(reasons);

            return Result.Ok(reasonsDtos);
        }

    }
}