using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Contracts.Versioning;
using Mileage.Client.Windows.Versioning;

namespace Mileage.Client.Windows.Windsor
{
    public class VersioningInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IVersionService>().ImplementedBy<VersionService>().LifestyleSingleton());
        }
    }
}