using AutoMapper;
using de_exceptional_closures.Helpers;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Closure;
using de_exceptional_closures.ViewModels.Home;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MyClosuresViewModel, ClosureReasonDto>()
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, MyClosuresViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, SubmittedViewModel>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, ViewViewModel>()
                 .ForMember(d => d.Approved,
                    o => o.MapFrom(s => HelperMethods.ApprovedOrNot(s.Approved)))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<IndexViewModel, ClosureReasonDto>()
                  .ForMember(d => d.CovidQ1, o => o.MapFrom(s => s.ReasonTypeId == (int)OtherReasonType.Covid ? s.CovidQ1 : null))
                  .ForMember(d => d.CovidQ2, o => o.MapFrom(s => s.ReasonTypeId == (int)OtherReasonType.Covid ? s.CovidQ2 : null))
                  .ForMember(d => d.CovidQ3, o => o.MapFrom(s => s.ReasonTypeId == (int)OtherReasonType.Covid ? s.CovidQ3 : null))
                  .ForMember(d => d.CovidQ4, o => o.MapFrom(s => s.ReasonTypeId == (int)OtherReasonType.Covid ? s.CovidQ4 : null))
                  .ForMember(d => d.CovidQ5, o => o.MapFrom(s => s.ReasonTypeId == (int)OtherReasonType.Covid ? s.CovidQ5 : null))
                     .ForMember(d => d.OtherReason, o => o.MapFrom(s => s.ReasonTypeId == (int)OtherReasonType.Other ? s.OtherReason : null))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, IndexViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}