using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Simulated;

namespace Mileage.Server.Infrastructure.Windsor
{
    public class FileSystemInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
           container.Register(
               Component.For<FileSystem>().Instance(FileSystem.Real()).LifestyleSingleton());
        }
    }
}