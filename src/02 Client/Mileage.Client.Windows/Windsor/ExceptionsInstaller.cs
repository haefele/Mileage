﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Mileage.Client.Contracts.Exceptions;
using Mileage.Client.Windows.Exceptions;

namespace Mileage.Client.Windows.Windsor
{
    public class ExceptionsInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IExceptionHandler>().ImplementedBy<ExceptionHandler>().LifestyleSingleton());
        }
    }
}