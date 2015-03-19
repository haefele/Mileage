using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Castle.Core.Logging;
using DevExpress.Xpf.Core.Native;
using LiteGuard;
using Mileage.Client.Contracts.Layout;
using Mileage.Client.Windows.Layout.Serializer;

namespace Mileage.Client.Windows.Layout
{
    public class LayoutManager : ILayoutManager
    {
        #region Fields
        private readonly ILayoutSerializer[] _layoutSerializers;
        private readonly ILayoutStorage _layoutStorage;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutManager"/> class.
        /// </summary>
        /// <param name="layoutSerializers">The layout serializers.</param>
        /// <param name="layoutStorage">The layout storage.</param>
        public LayoutManager(ILayoutSerializer[] layoutSerializers, ILayoutStorage layoutStorage)
        {
            Guard.AgainstNullArgument("layoutSerializers", layoutSerializers);
            Guard.AgainstNullArgument("layoutStorage", layoutStorage);

            this.Logger = NullLogger.Instance;

            this._layoutSerializers = layoutSerializers;
            this._layoutStorage = layoutStorage;
        }
        #endregion

        public Task SaveLayoutAsync(string layoutName, DependencyObject element)
        {
            return Task.Run(() =>
            {
                var enumerator = new VisualTreeEnumerator(element);

                while (enumerator.MoveNext())
                {
                    DependencyObject currentElement = enumerator.Current;

                    if (LayoutSettings.GetStopLayoutAnalysis(currentElement))
                        break;

                    var serializer = this._layoutSerializers.FirstOrDefault(f => f.ControlType.IsInstanceOfType(currentElement));

                    if (serializer == null)
                        continue;

                    string controlName = LayoutSettings.GetControlName(currentElement);

                    if (string.IsNullOrWhiteSpace(controlName))
                        continue;

                    using (var stream = new MemoryStream())
                    {
                        serializer.Save(currentElement, stream);

                        File.WriteAllBytes(Path.Combine("C:/", controlName), stream.ToArray());
                    }
                }
            });
        }

        public Task RestoreLayoutAsync(string layoutName, DependencyObject element)
        {
            throw new System.NotImplementedException();
        }
    }
}