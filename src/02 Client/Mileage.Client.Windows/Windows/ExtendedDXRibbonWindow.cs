using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Ribbon;

namespace Mileage.Client.Windows.Windows
{
    public class ExtendedDXRibbonWindow : DXRibbonWindow
    {
        #region Dependency Properties
        public static readonly DependencyProperty HideWindowButtonsProperty = DependencyProperty.Register(
            "HideWindowButtons", typeof (bool), typeof (ExtendedDXRibbonWindow), new PropertyMetadata(default(bool)));
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the window buttons should be hidden.
        /// </summary>
        public bool HideWindowButtons
        {
            get { return (bool) GetValue(HideWindowButtonsProperty); }
            set { SetValue(HideWindowButtonsProperty, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedDXRibbonWindow"/> class.
        /// </summary>
        public ExtendedDXRibbonWindow()
        {
            this.Loaded += this.OnLoaded;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Called when the <see cref="FrameworkElement.Loaded"/> event is executed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Loaded -= this.OnLoaded;

            if (this.HideWindowButtons)
            {
                var stackPanel = LayoutHelper.FindElementByName(this, "stackPanel") as StackPanel;
                if (stackPanel != null)
                {
                    stackPanel.Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion
    }
}