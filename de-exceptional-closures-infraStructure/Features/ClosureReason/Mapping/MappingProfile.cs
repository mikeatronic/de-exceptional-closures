using AutoMapper;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures_infraStructure.Features.ClosureReason.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClosureReasonDto, de_exceptional_closures_core.Entities.ClosureReason>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<de_exceptional_closures_core.Entities.ClosureReason, ClosureReasonDto>()
              .ForMember(d => d.ReasonType, o => o.MapFrom(s => s.ReasonType.Description))
              .ForMember(d => d.ApprovalType, o => o.MapFrom(s => s.ApprovalType.Description))
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<de_exceptional_closures_core.Entities.ClosureReason, ReasonTypeDto>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}