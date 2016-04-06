using AutoMapper;
using NSubstitute;
using TestHabanero.Controllers;
using TestHabenaro.Db.Interfaces;

namespace TestHabanero.Tests.Commons
{
    public class CarControllerBuilder
    {
        private ICarRepository _carRepository;
        private IMappingEngine _mappingEngine;

        public CarControllerBuilder()
        {
            _carRepository = Substitute.For<ICarRepository>();
            _mappingEngine = Substitute.For<IMappingEngine>();
        }

        public CarControllerBuilder WithCarRepository(ICarRepository borrowerRepository)
        {
            _carRepository = borrowerRepository;
            return this;
        }

        public CarControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
            return this;
        }

        public CarController Build()
        {
            return new CarController(_carRepository, _mappingEngine);
        }
    }
}
