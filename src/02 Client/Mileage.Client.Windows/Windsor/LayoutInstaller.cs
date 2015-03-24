using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Contracts.Layout;
using Mileage.Client.Windows.Layout;
using Mileage.Client.Windows.Layout.Serializer;

namespace Mileage.Client.Windows.Windsor
{
    public class LayoutInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILayoutManager>().ImplementedBy<LayoutManager>().LifestyleSingleton(),
                Component.For<LayoutCache>().LifestyleSingleton());

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<ILayoutSerializer>()
                    .WithServiceDefaultInterfaces()
                    .LifestyleSingleton());
        }
    }
}