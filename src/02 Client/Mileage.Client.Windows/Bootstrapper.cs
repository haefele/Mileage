using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Mileage.Client.Windows.Views.Login;
using Mileage.Client.Windows.Views.Shell;
using Mileage.Client.Windows.Windows;
using LayoutGroup = DevExpress.Xpf.Docking.LayoutGroup;

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
            DevExpressConventions.Install();
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

        #region Internal
        private class DevExpressConventions
        {
            public static void Install()
            {
                ReplaceGetNamedElements();
                InstallElementConventions();
            }

            private static void ReplaceGetNamedElements()
            {
                BindingScope.GetNamedElements = elementInScope =>
                {
                    var root = elementInScope;
                    var previous = elementInScope;
                    DependencyObject contentPresenter = null;
                    var routeHops = new Dictionary<DependencyObject, DependencyObject>();

                    while (true)
                    {
                        if (root == null)
                        {
                            root = previous;
                            break;
                        }

                        if (root is UserControl)
                            break;
#if !SILVERLIGHT
                        if (root is Page)
                        {
                            root = ((Page)root).Content as DependencyObject ?? root;
                            break;
                        }
#endif
                        if ((bool)root.GetValue(View.IsScopeRootProperty))
                            break;

                        if (root is ContentPresenter)
                            contentPresenter = root;
                        else if (root is ItemsPresenter && contentPresenter != null)
                        {
                            routeHops[root] = contentPresenter;
                            contentPresenter = null;
                        }

                        previous = root;
                        root = VisualTreeHelper.GetParent(previous);
                    }

                    var descendants = new List<FrameworkElement>();
                    var queue = new Queue<DependencyObject>();
                    queue.Enqueue(root);

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();
                        var currentElement = current as FrameworkElement;

                        if (currentElement != null && !string.IsNullOrEmpty(currentElement.Name))
                            descendants.Add(currentElement);

                        #region DevExpress specific

                        else
                        {
                            //BaseLayoutItem defines it's own Name property thus hiding FrameworkElement.Name
                            var currentLayoutElement = current as BaseLayoutItem;
                            if (currentLayoutElement != null && !string.IsNullOrEmpty(currentLayoutElement.Name))
                            {
                                descendants.Add(currentLayoutElement);

                                //Setting FrameworkElement.Name to BaseLayoutItem.Name so later Caliburn can reference the name and so no need to replace ViewModelBinder.BindProperties
                                currentElement.Name = currentLayoutElement.Name;
                            }
                        }

                        #endregion DevExpress specific

                        if (current is UserControl && current != root)
                            continue;

                        if (routeHops.ContainsKey(current))
                        {
                            queue.Enqueue(routeHops[current]);
                            continue;
                        }

                        var childCount = VisualTreeHelper.GetChildrenCount(current);
                        if (childCount > 0)
                        {
                            for (var i = 0; i < childCount; i++)
                            {
                                var childDo = VisualTreeHelper.GetChild(current, i);
                                queue.Enqueue(childDo);
                            }
                        }
                        else
                        {
                            var contentControl = current as ContentControl;
                            if (contentControl != null)
                            {
                                if (contentControl.Content is DependencyObject)
                                    queue.Enqueue(contentControl.Content as DependencyObject);

                                var headeredControl = contentControl as HeaderedContentControl;
                                if (headeredControl != null && headeredControl.Header is DependencyObject)
                                    queue.Enqueue(headeredControl.Header as DependencyObject);
                            }
                            else
                            {
                                var itemsControl = current as ItemsControl;
                                if (itemsControl != null)
                                {
                                    itemsControl.Items.OfType<DependencyObject>()
                                        .Apply(queue.Enqueue);

                                    var headeredControl = itemsControl as HeaderedItemsControl;
                                    if (headeredControl != null && headeredControl.Header is DependencyObject)
                                        queue.Enqueue(headeredControl.Header as DependencyObject);
                                }

                                #region DevExpress specific
                                else
                                {
                                    var dockLayoutManager = current as DockLayoutManager;
                                    if (dockLayoutManager != null && dockLayoutManager.LayoutRoot != null)
                                    {
                                        queue.Enqueue(dockLayoutManager.LayoutRoot);
                                    }

                                    var layoutGroup = current as LayoutGroup;
                                    if (layoutGroup != null && layoutGroup.Items != null)
                                    {
                                        layoutGroup.Items.OfType<DependencyObject>().Apply(queue.Enqueue);
                                    }

                                    var layoutPanel = current as LayoutPanel;
                                    if (layoutPanel != null && layoutPanel.Control != null)
                                    {
                                        queue.Enqueue(layoutPanel.Control);
                                    }

                                    var layoutItem = current as LayoutItem;
                                    if (layoutItem != null && layoutItem.Content != null)
                                    {
                                        queue.Enqueue(layoutItem.Content);
                                    }

                                    var gridControl = current as GridControl;
                                    if (gridControl != null && gridControl.View != null && string.IsNullOrWhiteSpace(gridControl.View.Name) == false)
                                    {
                                        queue.Enqueue(gridControl.View);
                                    }
                                }

                                #endregion DevExpress specific
                            }
                        }
                    }

                    return descendants;
                };
            }

            private static void InstallElementConventions()
            {
                //Grid
                ConventionManager.AddElementConvention<DataControlBase>(DataControlBase.ItemsSourceProperty, "DataContext", "Loaded");
                
                //LayoutControl
                ConventionManager.AddElementConvention<DataLayoutControl>(DataLayoutControl.CurrentItemProperty, "DataContext", "Loaded");
                
                //Editors
                ConventionManager.AddElementConvention<LookUpEditBase>(LookUpEditBase.ItemsSourceProperty, "SelectedItem", "SelectedIndexChanged")
                    .ApplyBinding = (viewModelType, path, property, element, convention) =>
                    {
                        var bindableProperty = convention.GetBindableProperty(element);
                        if (!ConventionManager.SetBindingWithoutBindingOrValueOverwrite(viewModelType, path, property, element, convention, bindableProperty))
                            return false;
                        ConventionManager.ConfigureSelectedItem(element, LookUpEditBase.SelectedItemProperty, viewModelType, path);
                        return true;
                    };
                ConventionManager.AddElementConvention<RangeBaseEdit>(RangeBaseEdit.ValueProperty, "Value", "EditValueChanged");
                ConventionManager.AddElementConvention<SpinEdit>(SpinEdit.ValueProperty, "Value", "EditValueChanged");
                ConventionManager.AddElementConvention<BaseEdit>(BaseEdit.EditValueProperty, "EditValue", "EditValueChanged");
            }
        }
        #endregion
    }
}
