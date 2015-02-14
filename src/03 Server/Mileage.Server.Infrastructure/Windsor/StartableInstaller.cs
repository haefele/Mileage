using Castle.Core;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mileage.Server.Infrastructure.Windsor
{
    public class StartableInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<StartableFacility>(f => f.DeferredStart());

            container.Register(
                Classes
                    .FromThisAssembly()
                    .BasedOn<IStartable>()
                    .LifestyleSingleton()
                    .WithServiceFirstInterface());
        }
    }
}