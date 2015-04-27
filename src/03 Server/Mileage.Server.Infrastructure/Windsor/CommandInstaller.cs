using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Commands;

namespace Mileage.Server.Infrastructure.Windsor
{
    public class CommandInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn(typeof (ICommandHandler<,>))
                    .WithServiceFromInterface()
                    .LifestyleTransient());

            container.Register(
                Component.For<ICommandExecutor>().ImplementedBy<CommandExecutor>().LifestyleSingleton(),
                Component.For<ICommandScope>().ImplementedBy<CommandScope>().LifestyleScoped());
        }
    }
}