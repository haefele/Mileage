using System.IO.Abstractions;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mileage.Server.Infrastructure.Windsor
{
    public class FileSystemInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
           container.Register(
               Component.For<IFileSystem>().Instance(new FileSystem()).LifestyleSingleton());
        }
    }
}