using AutoMapper;
using NSubstitute;
using TestHabanero.Controllers;
using TestHabenaro.Db.Interfaces;

namespace TestHabanero.Tests.Commons
{
    public class CarPartControllerBuilder
    {
        private  IPartRepository _partRepository;
        private  IMappingEngine _mappingEngine;
        private  ICarPartRepository _carPartRepository;
        private  ICarRepository _carRepository;
    

        public CarPartControllerBuilder()
        {
            _partRepository = Substitute.For<IPartRepository>();
            _mappingEngine = Substitute.For<IMappingEngine>();
            _carPartRepository = Substitute.For<ICarPartRepository>();
            _carRepository = Substitute.For<ICarRepository>();
        }

        public CarPartControllerBuilder WithPartRepository(IPartRepository partRepository)
        {
            _partRepository = partRepository;
            return this;
        }
        public CarPartControllerBuilder WithCarRepository(ICarRepository carRepository)
        {
            _carRepository = carRepository;
            return this;
        }
        public CarPartControllerBuilder WithCarPartRepository(ICarPartRepository carPartRepository)
        {
            _carPartRepository = carPartRepository;
            return this;
        }

        public CarPartControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
            return this;
        }

        public CarPartsController Build()
        {
            return new CarPartsController(_carPartRepository,_carRepository,_partRepository, _mappingEngine);
        }
    
}
}