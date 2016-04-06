using AutoMapper;
using NSubstitute;
using TestHabanero.Controllers;
using TestHabenaro.Db.Interfaces;

namespace TestHabanero.Tests.Commons
{
    public class PartControllerBuilder
    {
        private IPartRepository _partRepository;
        private IMappingEngine _mappingEngine;

        public PartControllerBuilder()
        {
            _partRepository = Substitute.For<IPartRepository>();
            _mappingEngine = Substitute.For<IMappingEngine>();
        }

        public PartControllerBuilder WithCarRepository(IPartRepository borrowerRepository)
        {
            _partRepository = borrowerRepository;
            return this;
        }

        public PartControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
            return this;
        }

        public PartController Build()
        {
            return new PartController(_partRepository, _mappingEngine);
        }
    }
}