using de_exceptional_closures_core.Dtos;
using System.Collections.Generic;

namespace de_exceptional_closures.ViewModels
{
    public class MyClosuresViewModel : BaseViewModel
    {
        public List<ClosureReasonDto> ClosureList { get; set; }
    }
}