using AutoMapper;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Features.Common;
using de_exceptional_closures_Infrastructure.Data;
using dss_common.Functional;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Commands
{
    public class CreateClosureReasonCommand : IRequest<Result<int>>
    {
        public ClosureReasonDto ClosureReasonDto { get; set; }
    }

    public class CreateClosureReasonCommandHandler : AbstractBaseHandler<CreateClosureReasonCommand, Result<int>>
    {
        public CreateClosureReasonCommandHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<CreateClosureReasonCommand> validator) : base(applicationDbContext, mapper, validator)
        {

        }

        public override async Task<Result<int>> Handle(CreateClosureReasonCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<int>(validationResult.ToString(","));

            var newParticipant = Mapper.Map<de_exceptional_closures_core.Entities.ClosureReason>(request.ClosureReasonDto);

            ApplicationDbContext.ClosureReason.Add(newParticipant);

            await ApplicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok(newParticipant.Id);
        }
    }
}