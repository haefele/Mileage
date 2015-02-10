using System.Dynamic;
using System.Windows;

namespace Mileage.Client.Windows.Windows
{
    public static class WindowSetting
    {
        #region Methods
        public static dynamic AutoSizeNoResize()
        {
            dynamic settings = AutoSize();
            settings.ResizeMode = ResizeMode.NoResize;

            return settings;
        }
        public static dynamic AutoSizeResize()
        {
            dynamic settings = AutoSize();
            settings.ResizeMode = ResizeMode.CanResize;

            return settings;
        }

        public static dynamic AutoSizeResizeMinSize(int width, int height)
        {
            dynamic settings = AutoSizeResize();
            settings.MinWidth = width;
            settings.MinHeight = height;

            return settings;
        }
        public static dynamic FixedSizeNoResize(int width, int height)
        {
            dynamic settings = FixedSize(width, height);
            settings.ResizeMode = ResizeMode.NoResize;
            settings.SizeToContent = SizeToContent.Manual;

            return settings;
        }
        public static dynamic FixedSizeResize(int width, int height)
        {
            dynamic settings = FixedSize(width, height);
            settings.ResizeMode = ResizeMode.CanResize;
            settings.SizeToContent = SizeToContent.Manual;

            return settings;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns a settings object with a fixed <paramref name="width"/> and <paramref name="height"/>.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private static dynamic FixedSize(int width, int height)
        {
            dynamic settings = new ExpandoObject();
            settings.Width = width;
            settings.Height = height;
            settings.MinWidth = width;
            settings.MinHeight = height;

            return settings;
        }
        /// <summary>
        /// Returns a settings object that automatically adjusts it's size to it's content.
        /// </summary>
        private static dynamic AutoSize()
        {
            dynamic settings = new ExpandoObject();
            settings.SizeToContent = SizeToContent.WidthAndHeight;

            return settings;
        }
        #endregion
    }
}