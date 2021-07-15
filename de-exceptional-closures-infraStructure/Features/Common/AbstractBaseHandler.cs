using AutoMapper;
using de_exceptional_closures_Infrastructure.Data;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace de_exceptional_closures_infraStructure.Features.Common
{
    public abstract class AbstractBaseHandler<T, TR> : IRequestHandler<T, TR> where T : IRequest<TR>
    {
        protected readonly ApplicationDbContext ApplicationDbContext;
        protected readonly IValidator<T> Validator;
        protected readonly IMapper Mapper;

        protected AbstractBaseHandler(ApplicationDbContext applicationDbContext, IMapper mapper, IValidator<T> validator)
        {
            Mapper = mapper;
            ApplicationDbContext = applicationDbContext;
            Validator = validator;
        }
        public abstract Task<TR> Handle(T request, CancellationToken cancellationToken);
    }
}