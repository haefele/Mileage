using System;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Windows.Views;
using Mileage.Client.Windows.Views.Shell;
using Mileage.Shared.Extensions;

namespace Mileage.Client.Windows.Windsor
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromThisAssembly()
                    .BasedOn<ReactivePropertyChangedBase>()
                    .WithServiceSelf()
                    //Register all ShellItemViewModels
                    .ConfigureFor<ShellItemViewModel>(f => 
                        f.Forward<ShellItemViewModel>())
                    //Ignore the ActiveItem properties of conductors
                    .ConfigureIf(f => typeof(MileageConductorBaseWithActiveItem<>).IsAssignableFromGenericType(f.Implementation), f => 
                        f.PropertiesIgnore(d => d.Name == "ActiveItem"))
                    .LifestyleTransient());
        }
    }
}