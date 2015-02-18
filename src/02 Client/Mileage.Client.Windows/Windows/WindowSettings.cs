using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;

namespace Mileage.Client.Windows.Windows
{
    public class WindowSettings : IDictionary<string, object>
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

        #region Implementation of IDictionary<string, object>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this._settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this._settings.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this._settings.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this._settings.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new InvalidOperationException();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new InvalidOperationException();
        }

        public int Count
        {
            get { return this._settings.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool ContainsKey(string key)
        {
            return this._settings.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            this._settings.Add(key, value);
        }

        public bool Remove(string key)
        {
            return this._settings.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this._settings.TryGetValue(key, out value);
        }

        public object this[string key]
        {
            get { return this._settings[key]; }
            set { this._settings[key] = value; }
        }

        public ICollection<string> Keys
        {
            get { return this._settings.Keys; }
        }

        public ICollection<object> Values
        {
            get { return this._settings.Values; }
        }
        #endregion
    }
}