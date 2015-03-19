using System;
using System.IO;
using System.Windows;

namespace Mileage.Client.Windows.Layout.Serializer
{
    /// <summary>
    /// A base class to have a more convenient way to implement a <see cref="ILayoutSerializer"/>.
    /// </summary>
    /// <typeparam name="T">The type of control this layoutSerializer can handle.</typeparam>
    public abstract class LayoutSerializerBase<T> : ILayoutSerializer
        where T : class
    {
        #region Properties
        /// <summary>
        /// Gets the type of controls that can be saved and restored with this serializer.
        /// </summary>
        public Type ControlType
        {
            get { return typeof (T); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Saves the layout of the specified <paramref name="control" /> into the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout saved.</param>
        /// <param name="stream">The stream that should contain the layout data.</param>
        protected abstract void Save(T control, Stream stream);
        /// <summary>
        /// Restores the layout of the specified <paramref name="control" /> with the data from the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout restored.</param>
        /// <param name="stream">The stream that contains the layout data.</param>
        protected abstract void Restore(T control, Stream stream);
        #endregion

        #region Implementation of ILayoutSerializer
        /// <summary>
        /// Saves the layout of the specified <paramref name="control" /> into the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout saved.</param>
        /// <param name="stream">The stream that should contain the layout data.</param>
        void ILayoutSerializer.Save(DependencyObject control, Stream stream)
        {
            this.Save(control as T, stream);
        }
        /// <summary>
        /// Restores the layout of the specified <paramref name="control" /> with the data from the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout restored.</param>
        /// <param name="stream">The stream that contains the layout data.</param>
        void ILayoutSerializer.Restore(DependencyObject control, Stream stream)
        {
            this.Restore(control as T, stream);
        }
        #endregion
    }
}