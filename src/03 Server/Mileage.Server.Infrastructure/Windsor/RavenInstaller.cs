using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Entities.Mileage;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;
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

                AssembliesDirectory = Path.Combine(".", "Database", "Assemblies"),
                EmbeddedFilesDirectory = Path.Combine(".", "Database", "Files"),
                DataDirectory = Path.Combine(".", "Database", "Data"),
                CompiledIndexCacheDirectory = Path.Combine(".", "Database", "Raven", "CompiledIndexCache"),
                PluginsDirectory = Path.Combine(".", "Database", "Plugins"),
                MaxSecondsForTaskToWaitForDatabaseToLoad = 20,
            };
            config.Settings.Add("Raven/CompiledIndexCacheDirectory", config.CompiledIndexCacheDirectory);

            var ravenDbServer = new RavenDbServer(config);

            ravenDbServer.Initialize();

            ravenDbServer.DocumentStore.DefaultDatabase = Dependency.OnAppSettingsValue("Mileage/RavenName").Value;
            ravenDbServer.DocumentStore.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(ravenDbServer.DocumentStore.DefaultDatabase);

            ravenDbServer.FilesStore.DefaultFileSystem = Dependency.OnAppSettingsValue("Mileage/RavenName").Value;
            ravenDbServer.FilesStore.AsyncFilesCommands.Admin.EnsureFileSystemExistsAsync(ravenDbServer.FilesStore.DefaultFileSystem).Wait();
            
            if (bool.Parse(Dependency.OnAppSettingsValue("Mileage/EnableRavenHttpServer").Value))
            {
                ravenDbServer.EnableHttpServer();
            }

            this.CustomizeRavenDocumentStore(ravenDbServer.DocumentStore);

            return ravenDbServer;
        }
        /// <summary>
        /// Customizes the raven database.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        private void CustomizeRavenDocumentStore(IDocumentStore documentStore)
        {
            documentStore.Conventions.RegisterAsyncIdConvention<AuthenticationData>((databaseName, commands, entity) =>
            {
                return Task.FromResult(AuthenticationData.CreateId(entity.UserId));
            });
            documentStore.Conventions.RegisterAsyncIdConvention<AuthenticationToken>((databaseName, commands, entity) =>
            {
                return Task.FromResult(AuthenticationToken.CreateId(entity.Token));
            });
            documentStore.Conventions.RegisterAsyncIdConvention<MileageInternalSettings>((databaseName, commands, entity) =>
            {
                return Task.FromResult(MileageInternalSettings.CreateId());
            });
        }
        #endregion
    }
}