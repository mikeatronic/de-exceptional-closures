using AutoMapper;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Closure;
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

            CreateMap<ClosureReasonDto, EditViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EditViewModel, ClosureReasonDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, ViewViewModel>()
                 .ForMember(d => d.Approved,
                    o => o.MapFrom(s => s.Approved.Value ? "Yes" : "No"))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RequestClosureViewModel, ClosureReasonDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, RequestClosureViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, EditReasonViewModel>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EditReasonViewModel, ClosureReasonDto>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, EditDateToViewModel>()
                .ForMember(d => d.DateToDay, o => o.MapFrom(s => s.DateTo.Value.Day))
                .ForMember(d => d.DateToMonth, o => o.MapFrom(s => s.DateTo.Value.Month))
                .ForMember(d => d.DateToYear, o => o.MapFrom(s => s.DateTo.Value.Year))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, EditDateFromViewModel>()
                .ForMember(d => d.DateFromDay, o => o.MapFrom(s => s.DateFrom.Day))
                .ForMember(d => d.DateFromMonth, o => o.MapFrom(s => s.DateFrom.Month))
                .ForMember(d => d.DateFromYear, o => o.MapFrom(s => s.DateFrom.Year))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}