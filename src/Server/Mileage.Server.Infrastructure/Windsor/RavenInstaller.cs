using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Extensions;
using Raven.Client.FileSystem;
using Raven.Client.Indexes;
using Raven.Database.Config;
using Raven.Server;

namespace Mileage.Server.Infrastructure.Windsor
{
    public class RavenInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            RavenDbServer server = this.CreateRavenDbServer();

            container.Register(
                Component.For<IDocumentStore>().Instance(server.DocumentStore).LifestyleSingleton(),
                Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession()).LifestyleScoped(),
                Component.For<IFilesStore>().Instance(server.FilesStore).LifestyleSingleton(),
                Component.For<IAsyncFilesSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IFilesStore>().OpenAsyncSession()).LifestyleScoped());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the RavenDB server.
        /// </summary>
        private RavenDbServer CreateRavenDbServer()
        {
            var config = new RavenConfiguration
            {
                Port = int.Parse(Dependency.OnAppSettingsValue("Mileage/RavenHttpServerPort").Value),

                DataDirectory = Path.Combine(".", "Database", "Data"),
                CompiledIndexCacheDirectory = Path.Combine(".", "Database", "Raven"),
                PluginsDirectory = Path.Combine(".", "Database", "Plugins"),
            };
            config.Settings.Add("Raven/CompiledIndexCacheDirectory", config.CompiledIndexCacheDirectory);

            var ravenDbServer = new RavenDbServer(config);

            ravenDbServer.Initialize();

            ravenDbServer.DocumentStore.DefaultDatabase = Dependency.OnAppSettingsValue("Mileage/RavenName").Value;
            ravenDbServer.DocumentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(ravenDbServer.DocumentStore.DefaultDatabase);

            ravenDbServer.FilesStore.DefaultFileSystem = Dependency.OnAppSettingsValue("Mileage/RavenName").Value;
            ravenDbServer.FilesStore.AsyncFilesCommands.Admin.EnsureFileSystemExistsAsync(ravenDbServer.FilesStore.DefaultFileSystem).Wait();

            IndexCreation.CreateIndexes(this.GetType().Assembly, ravenDbServer.DocumentStore);

            if (bool.Parse(Dependency.OnAppSettingsValue("Mileage/EnableRavenHttpServer").Value))
            {
                ravenDbServer.EnableHttpServer();
            }

            return ravenDbServer;
        }
        #endregion
    }
}