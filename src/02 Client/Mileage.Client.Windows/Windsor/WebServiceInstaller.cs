using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Contracts.WebServices;
using Mileage.Client.Windows.WebServices;

namespace Mileage.Client.Windows.Windsor
{
    public class WebServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<WebServiceClient>().LifestyleSingleton(),
                Component.For<IAuthenticationClient>().ImplementedBy<AuthenticationClient>().LifestyleSingleton(),
                Component.For<IUsersClient>().ImplementedBy<UsersClient>().LifestyleSingleton(),
                Component.For<ILayoutClient>().ImplementedBy<LayoutClient>().LifestyleSingleton(),
                Component.For<MileageClient>().LifestyleSingleton().DependsOn(Dependency.OnAppSettingsValue("baseAddress", "Mileage/WebServiceAddress")),
                Component.For<Session>().LifestyleSingleton());
        }
    }
}