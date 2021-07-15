using AutoMapper;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures_infraStructure.Features.ReasonType.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReasonTypeDto, de_exceptional_closures_core.Entities.ReasonType>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<de_exceptional_closures_core.Entities.ReasonType, ReasonTypeDto>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}