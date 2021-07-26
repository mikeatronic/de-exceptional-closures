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

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Commands
{
    public class UpdateClosureReasonCommand : IRequest<Result<int>>
    {
        public ClosureReasonDto ClosureReasonDto { get; set; }
    }

    public class UpdateClosureReasonCommandHandler : AbstractBaseHandler<UpdateClosureReasonCommand, Result<int>>
    {
        public UpdateClosureReasonCommandHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<UpdateClosureReasonCommand> validator) : base(applicationDbContext, mapper, validator)
        {

        }

        public override async Task<Result<int>> Handle(UpdateClosureReasonCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<int>(validationResult.ToString(","));

            var maybeClosure = (await ApplicationDbContext.ClosureReason.SingleOrDefaultAsync(d => d.Id == request.ClosureReasonDto.Id)).ToMaybe();

            if (!maybeClosure.HasValue)
                return Result.Fail<int>("Closure reason not found");

            Mapper.Map(request.ClosureReasonDto, maybeClosure.Value);

            await ApplicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(maybeClosure.Value.Id);
        }
    }
}