using Caliburn.Micro.ReactiveUI;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Mileage.Client.Windows.Windsor
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .BasedOn<ReactivePropertyChangedBase>()
                    .WithServiceSelf()
                    .LifestyleTransient());
        }
    }
}