using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LiteDB;
using Mileage.Client.Contracts.Storage;
using Mileage.Client.Windows.Storage;

namespace Mileage.Client.Windows.Windsor
{
    public class LiteDatabaseInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDataStorage>().ImplementedBy<LiteDatabaseStorage>()
                    .DependsOn(Dependency.OnValue("filePath", Config.EmbeddedDatabaseName.GetValue())));
        }
    }
}