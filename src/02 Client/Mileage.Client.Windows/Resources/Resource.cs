using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Castle.Core.Logging;

namespace Mileage.Client.Windows.Resources
{
    public static class Resource
    {
        public static class Icon
        {
            public static ImageSource DocumentEmpty
            {
                get { return GetImage("document_empty.png"); }
            }

            public static ImageSource DocumentInfo
            {
                get { return GetImage("document_info.png"); }
            }

            public static ImageSource DocumentInspector
            {
                get { return GetImage("document_inspector.png"); }
            }

            #region Internal            
            /// <summary>
            /// The image cache.
            /// This cache is immutable, we create a new one if we need to add an item.
            /// </summary>
            private static Dictionary<string, ImageSource> _imageCache = new Dictionary<string, ImageSource>();
            /// <summary>
            /// Returns the cached image or tries to create it.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            private static ImageSource GetImage(string fileName)
            {
                if (_imageCache.ContainsKey(fileName) == false)
                {
                    try
                    {
                        Uri imageUri = new Uri("pack://application:,,,/Resources/Icons/" + fileName);
                        ImageSource image = new BitmapImage(imageUri);

                        _imageCache = new Dictionary<string, ImageSource>(_imageCache)
                        {
                            {fileName, image}
                        };
                    }
                    catch (Exception exception)
                    {
                        var loggerFactory = IoC.Get<ILoggerFactory>();
                        var logger = loggerFactory.Create(typeof (Icon));

                        logger.Error(string.Format("Exception while creating image {0}.", fileName), exception);
                        throw;
                    }
                }

                return _imageCache[fileName];
            }
            #endregion
        }
    }
}