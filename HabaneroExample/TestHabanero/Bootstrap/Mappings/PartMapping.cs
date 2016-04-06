using AutoMapper;
using TestHabanero.BO;
using TestHabanero.Models;

namespace TestHabanero.Bootstrap.Mappings
{
    public class PartMapping : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<PartViewModel, Part>()
                .ForMember(b => b.PartId, x => x.Ignore());


            Mapper.CreateMap<Part, PartViewModel>();
        }
    }
}