﻿using AutoMapper;
using de_exceptional_closures.Helpers;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Closure;
using de_exceptional_closures.ViewModels.Home;
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
                    o => o.MapFrom(s => HelperMethods.ApprovedOrNot(s.Approved)))
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
                .ForMember(d => d.DateFromDay, o => o.MapFrom(s => s.DateFrom.Value.Day))
                .ForMember(d => d.DateFromMonth, o => o.MapFrom(s => s.DateFrom.Value.Month))
                .ForMember(d => d.DateFromYear, o => o.MapFrom(s => s.DateFrom.Value.Year))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EditDateToViewModel, ClosureReasonDto>()
          .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<EditDateFromViewModel, ClosureReasonDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<IndexViewModel, ClosureReasonDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ClosureReasonDto, IndexViewModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}