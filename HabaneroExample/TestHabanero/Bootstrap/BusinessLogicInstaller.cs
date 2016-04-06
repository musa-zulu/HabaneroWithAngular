using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TestHabenaro.Db.Interfaces;
using TestHabenaro.DB;

namespace TestHabanero.Bootstrap
{
    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICarRepository>().ImplementedBy<CarRepository>());
            container.Register(Component.For<IPartRepository>().ImplementedBy<PartRepository>());
            container.Register(Component.For<ICarPartRepository>().ImplementedBy<CarPartRepository>());
        }
    }
}