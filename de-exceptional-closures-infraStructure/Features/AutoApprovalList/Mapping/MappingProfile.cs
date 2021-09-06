using AutoMapper;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures_infraStructure.Features.AutoApprovalList.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutoApprovalListDto, de_exceptional_closures_core.Entities.AutoApprovalList>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<de_exceptional_closures_core.Entities.AutoApprovalList, AutoApprovalListDto>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}