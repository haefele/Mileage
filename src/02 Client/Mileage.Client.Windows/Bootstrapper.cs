using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Mileage.Client.Windows.Views.Login;
using Mileage.Client.Windows.Views.Shell;
using Mileage.Client.Windows.Windows;

namespace Mileage.Client.Windows
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Fields
        private IWindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.Initialize();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            this._container = new WindsorContainer();
            this._container.Install(FromAssembly.This());

            this.ConfigureDevExpressTheme();
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        protected override object GetInstance(Type service, string key)
        {
            if (this._container.Kernel.HasComponent(service) == false)
                return base.GetInstance(service, key);

            return this._container.Resolve(service);
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (this._container.Kernel.HasComponent(service) == false)
                return base.GetAllInstances(service);

            return this._container.ResolveAll(service).Cast<object>();
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            IEnumerable<PropertyInfo> propertiesToInject = instance
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.CanWrite && this._container.Kernel.HasComponent(f.PropertyType));

            foreach (var property in propertiesToInject)
            {
                property.SetValue(instance, this._container.Resolve(property.PropertyType));
            }
        }
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            bool loggedOut = true;

            //While we log us out from the shell-view we show the login view again
            while (loggedOut)
            {
                //We logged us out if we successfully logged us in and then logged us out in the shell
                loggedOut = this.ShowLoginView() && this.ShowShellView();
            }

            Application.Shutdown();
        }
        /// <summary>
        /// Override this to add custom behavior on exit.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnExit(object sender, EventArgs e)
        {
            this._container.Dispose();
        }
        /// <summary>
        /// Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var loggerFactory = this._container.Resolve<ILoggerFactory>();

            ILogger logger = loggerFactory.Create("Default");
            logger.Error("An unhandled error occured.", e.Exception);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Configures the DevExpress theme.
        /// </summary>
        private void ConfigureDevExpressTheme()
        {
            ThemeManager.ApplicationThemeName = "Office2013";
        }
        /// <summary>
        /// Shows the login view and returns whether the login was successfull.
        /// </summary>
        private bool ShowLoginView()
        {
            var windowManager = this._container.Resolve<IWindowManager>();
            var loginViewModel = this._container.Resolve<LoginViewModel>();

            bool? loggedIn = windowManager.ShowDialog(loginViewModel, null, WindowSettings.With().AutoSize().NoIcon().NoResize());

            this._container.Release(loginViewModel);

            return loggedIn.GetValueOrDefault();
        }
        /// <summary>
        /// Shows the shell view and returns whether the user logged himself out.
        /// </summary>
        private bool ShowShellView()
        {
            var windowManager = this._container.Resolve<IWindowManager>();
            var shellViewModel = this._container.Resolve<ShellViewModel>();

            bool? loggedOut = windowManager.ShowDialog(shellViewModel, null, WindowSettings.With().FixedSize(1280, 720).Resize().NoIcon());
            
            this._container.Release(shellViewModel);

            return loggedOut.GetValueOrDefault();
        }
        #endregion
    }
}
