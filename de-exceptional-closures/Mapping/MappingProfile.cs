using AutoMapper;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures_core.Dtos;

namespace de_exceptional_closures.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PreApprovedViewModel, ClosureReasonDto>();
            CreateMap<ClosureReasonDto, PreApprovedViewModel>();
                
        }
    }
}