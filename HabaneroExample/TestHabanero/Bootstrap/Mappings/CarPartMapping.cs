using AutoMapper;
using TestHabanero.BO;
using TestHabanero.Models;

namespace TestHabanero.Bootstrap.Mappings
{
    public class CarPartMapping : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<CarPartsIndexViewModel, CarPart>();
            Mapper.CreateMap<CarPartsViewModel, CarPart>();
            Mapper.CreateMap<CarPart, CarPartsIndexViewModel>();
            Mapper.CreateMap<CarPart, CarPartsViewModel>();
                             
        }
    }
}