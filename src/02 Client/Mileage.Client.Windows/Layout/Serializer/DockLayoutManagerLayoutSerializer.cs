using System;
using System.IO;
using System.Windows;
using DevExpress.Xpf.Docking;

namespace Mileage.Client.Windows.Layout.Serializer
{
    /// <summary>
    /// A layout serializer for the <see cref="DockLayoutManager"/> class.
    /// </summary>
    public class DockLayoutManagerLayoutSerializer : LayoutSerializerBase<DockLayoutManager>
    {
        /// <summary>
        /// Saves the layout of the specified <paramref name="control" /> into the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout saved.</param>
        /// <param name="stream">The stream that should contain the layout data.</param>
        protected override void Save(DockLayoutManager control, Stream stream)
        {
            control.SaveLayoutToStream(stream);
        }
        /// <summary>
        /// Restores the layout of the specified <paramref name="control" /> with the data from the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout restored.</param>
        /// <param name="stream">The stream that contains the layout data.</param>
        protected override void Restore(DockLayoutManager control, Stream stream)
        {
            control.RestoreLayoutFromStream(stream);
        }
    }
}