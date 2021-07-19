using AutoMapper;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PreApprovedViewModel, ClosureReasonDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, PreApprovedViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ApprovalRequiredViewModel, ClosureReasonDto>()
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, ApprovalRequiredViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<MyClosuresViewModel, ClosureReasonDto>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, MyClosuresViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}