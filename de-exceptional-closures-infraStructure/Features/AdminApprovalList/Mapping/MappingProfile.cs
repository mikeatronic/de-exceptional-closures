using AutoMapper;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures_infraStructure.Features.AdminApprovalList.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdminApprovalListDto, de_exceptional_closures_core.Entities.AdminApprovalList>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<de_exceptional_closures_core.Entities.AdminApprovalList, AdminApprovalListDto>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}