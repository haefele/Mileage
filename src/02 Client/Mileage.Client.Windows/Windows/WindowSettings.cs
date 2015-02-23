using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;

namespace Mileage.Client.Windows.Windows
{
    public class WindowSettings : Dictionary<string, object>
    {
        #region Fields
        private readonly Dictionary<string, object> _settings;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSettings"/> class.
        /// </summary>
        private WindowSettings()
        {
            this._settings = new Dictionary<string, object>();
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Allows the user to create a new <see cref="WindowSettings"/> with the following settings.
        /// </summary>
        public static WindowSettings With()
        {
            return new WindowSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configures the window to autosize to fit it's content.
        /// </summary>
        public WindowSettings AutoSize()
        {
            this._settings[Window.SizeToContentProperty.Name] = SizeToContent.WidthAndHeight;

            return this;
        }
        /// <summary>
        /// Configures the window to have the specified size.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public WindowSettings FixedSize(int width, int height)
        {
            this._settings[FrameworkElement.WidthProperty.Name] = width;
            this._settings[FrameworkElement.HeightProperty.Name] = height;
            this._settings[FrameworkElement.MinWidthProperty.Name] = width;
            this._settings[FrameworkElement.MinHeightProperty.Name] = height;

            return this;
        }
        /// <summary>
        /// Configures the window to allow resizing.
        /// </summary>
        public WindowSettings Resize()
        {
            this._settings[Window.ResizeModeProperty.Name] = ResizeMode.CanResize;

            return this;
        }
        /// <summary>
        /// Configures the window to disallow resizing.
        /// </summary>
        public WindowSettings NoResize()
        {
            this._settings[Window.ResizeModeProperty.Name] = ResizeMode.NoResize;

            return this;
        }

        /// <summary>
        /// Configures the window to not have window buttons.
        /// </summary>
        public WindowSettings NoWindowButtons()
        {
            this._settings[ExtendedDXWindow.HideWindowButtonsProperty.Name] = true;

            return this;
        }
        /// <summary>
        /// Configures the window to not have a icon.
        /// </summary>
        public WindowSettings NoIcon()
        {
            this._settings[DXWindow.ShowIconProperty.Name] = false;

            return this;
        }
        #endregion
    }
}