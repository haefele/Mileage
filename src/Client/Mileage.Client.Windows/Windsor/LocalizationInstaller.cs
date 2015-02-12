using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Contracts.Localization;
using Mileage.Client.Windows.Localization;

namespace Mileage.Client.Windows.Windsor
{
    public class LocalizationInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILocalizationManager>().ImplementedBy<LocalizationManager>().LifestyleSingleton());
        }
    }
}