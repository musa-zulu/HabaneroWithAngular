using AutoMapper;
using TestHabanero.BO;
using TestHabanero.Models;

namespace TestHabanero.Bootstrap.Mappings
{
    public class CarMappings : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<CarViewModel, Car>()
                .ForMember(b => b.CarId, x => x.Ignore());


            Mapper.CreateMap<Car, CarViewModel>();
        }
    }
}