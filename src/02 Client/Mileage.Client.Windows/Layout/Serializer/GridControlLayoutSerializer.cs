using System.IO;
using DevExpress.Xpf.Grid;

namespace Mileage.Client.Windows.Layout.Serializer
{
    /// <summary>
    /// A layout serializer for the <see cref="GridControl"/> class.
    /// </summary>
    public class GridControlLayoutSerializer : LayoutSerializerBase<GridControl>
    {
        /// <summary>
        /// Saves the layout of the specified <paramref name="control" /> into the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout saved.</param>
        /// <param name="stream">The stream that should contain the layout data.</param>
        protected override void Save(GridControl control, Stream stream)
        {
            control.SaveLayoutToStream(stream);
        }
        /// <summary>
        /// Restores the layout of the specified <paramref name="control" /> with the data from the specified <paramref name="stream" />.
        /// </summary>
        /// <param name="control">The control to have it's layout restored.</param>
        /// <param name="stream">The stream that contains the layout data.</param>
        protected override void Restore(GridControl control, Stream stream)
        {
            control.RestoreLayoutFromStream(stream);
        }
    }
}