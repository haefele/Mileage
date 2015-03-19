using System;
using System.IO;
using System.Windows;

namespace Mileage.Client.Windows.Layout.Serializer
{
    /// <summary>
    /// Provides methods to save and restore the layout of <see cref="ControlType"/> controls.
    /// </summary>
    public interface ILayoutSerializer
    {
        /// <summary>
        /// Gets the type of controls that can be saved and restored with this serializer.
        /// </summary>
        Type ControlType { get; }

        /// <summary>
        /// Saves the layout of the specified <paramref name="control"/> into the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="control">The control to have it's layout saved.</param>
        /// <param name="stream">The stream that should contain the layout data.</param>
        void Save(DependencyObject control, Stream stream);
        /// <summary>
        /// Restores the layout of the specified <paramref name="control"/> with the data from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="control">The control to have it's layout restored.</param>
        /// <param name="stream">The stream that contains the layout data.</param>
        void Restore(DependencyObject control, Stream stream);
    }
}