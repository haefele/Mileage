using Caliburn.Micro;
using Castle.Core;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Windows.Windows;

namespace Mileage.Client.Windows.Windsor
{
    public class CaliburnInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWindowManager>().ImplementedBy<MileageWindowManager>().LifestyleSingleton(),
                Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifestyleSingleton());

            container.AddFacility<EventRegistrationFacility>();
        }

        #region Internal
        /// <summary>
        /// A castle windsor facility that automatically registers Caliburn.Micro event handler instances.
        /// </summary>
        private class EventRegistrationFacility : AbstractFacility
        {
            /// <summary>
            /// The custom initialization for the Facility.
            /// </summary>
            protected override void Init()
            {
                this.Kernel.ComponentCreated += this.ComponentCreated;
            }
            /// <summary>
            /// Executed when a component was created.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="instance">The instance.</param>
            private void ComponentCreated(ComponentModel model, object instance)
            {
                if (instance is IHandle == false)
                    return;

                var eventAggregator = this.Kernel.Resolve<IEventAggregator>();
                eventAggregator.Subscribe(instance);
            }
        }
        #endregion
    }
}