using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;
using Caliburn.Micro;
using Castle.Core.Logging;
using DevExpress.Xpf.Core.Native;
using DevExpress.XtraPrinting.Native.LayoutAdjustment;
using LiteGuard;
using Mileage.Client.Contracts.Layout;
using Mileage.Client.Windows.Layout.Serializer;
using Mileage.Client.Windows.WebServices;
using Mileage.Shared.Entities.Layout;
using Mileage.Shared.Entities.Users;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace Mileage.Client.Windows.Layout
{
    public class LayoutManager : ILayoutManager
    {
        #region Fields
        private readonly ILayoutSerializer[] _layoutSerializers;
        private readonly WebServiceClient _webServiceClient;

        private readonly LayoutCache _layoutCache;
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
        /// <param name="webServiceClient">The webservice client.</param>
        /// <param name="layoutCache">The layout cache.</param>
        public LayoutManager(ILayoutSerializer[] layoutSerializers, WebServiceClient webServiceClient, LayoutCache layoutCache)
        {
            Guard.AgainstNullArgument("layoutSerializers", layoutSerializers);
            Guard.AgainstNullArgument("webServiceClient", webServiceClient);
            Guard.AgainstNullArgument("layoutCache", layoutCache);

            this.Logger = NullLogger.Instance;

            this._layoutSerializers = layoutSerializers;
            this._webServiceClient = webServiceClient;
            this._layoutCache = layoutCache;
        }
        #endregion

        #region Methods
        public async Task SaveLayoutAsync(User user, string layoutName, DependencyObject control)
        {
            if (user == null)
                return;
            
            var layoutData = new Dictionary<string, byte[]>();

            this.ExecuteForFoundElements(control, (serializer, element, controlName) =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    serializer.Save(element, memoryStream);
                    layoutData.Add(controlName, memoryStream.ToArray());
                }
            });

            if (this._layoutCache.HasChanged(layoutName, layoutData) == false)
                return;
            
            //Save the data in the background
            var saveResponse = await this._webServiceClient.LayoutClient.SaveLayout(layoutName, layoutData).ConfigureAwait(false);
            if (saveResponse.StatusCode != HttpStatusCode.Created)
            {
                HttpError error = await saveResponse.Content.ReadAsAsync<HttpError>();
                this.Logger.DebugFormat(error.Message);
            }
        }
        public async Task RestoreLayoutAsync(User user, string layoutName, DependencyObject control)
        {
            if (user == null)
                return;

            Dictionary<string, byte[]> cachedLayout = this._layoutCache.Get(layoutName) ?? await this.GetLayoutFromWebService(layoutName);

            if (cachedLayout == null)
                return;

            this.ExecuteForFoundElements(control, (serializer, element, controlName) =>
            {
                if (cachedLayout.ContainsKey(controlName) == false)
                    return;

                using (var memoryStream = new MemoryStream(cachedLayout[controlName]))
                {
                    serializer.Restore(element, memoryStream);
                }
            });
        }
        #endregion

        #region Private Methods
        private void ExecuteForFoundElements(DependencyObject element, Action<ILayoutSerializer, DependencyObject, string> action)
        {
            var enumerator = new VisualTreeEnumerator(element);
            var ignoredParentElements = new List<DependencyObject>();

            while (enumerator.MoveNext())
            {
                //Get the current control
                DependencyObject currentElement = enumerator.Current;
                string currentElementName = currentElement is FrameworkElement ?
                        ((FrameworkElement)currentElement).Name :
                        string.Empty;

                //See if we should ignore this control
                if (LayoutSettings.GetIgnoreChildControls(currentElement))
                {
                    this.Logger.DebugFormat("Ignoring all children of this control {0} (Name: {1}).", currentElement.GetType(), currentElementName);

                    ignoredParentElements.Add(currentElement);
                    continue;
                }

                //See if there is an serializer for our type
                var serializer = this._layoutSerializers.FirstOrDefault(f => f.ControlType.IsInstanceOfType(currentElement));

                if (serializer == null)
                    continue;

                //See if any of our parents was ignored
                if (enumerator.GetVisualParents().Any(ignoredParentElements.Contains))
                {
                    this.Logger.DebugFormat("Ignoring this control {0} (Name: {1}) because a parent was ignored.", currentElement.GetType(), currentElementName);
                    continue;
                }

                //Get the control name
                string controlName = LayoutSettings.GetLayoutControlName(currentElement);

                if (string.IsNullOrWhiteSpace(controlName))
                {
                    this.Logger.DebugFormat("Wanted to work with layout of {0} (Name: {1}) but it has no name.", currentElement.GetType(), currentElementName);
                    continue;
                }
                
                //Execute the actual action
                action(serializer, currentElement, controlName);
            }
        }
        
        private async Task<Dictionary<string, byte[]>> GetLayoutFromWebService(string layoutName)
        {
            var getLayoutResponse = await this._webServiceClient.LayoutClient.GetLayout(layoutName);
            if (getLayoutResponse.StatusCode != HttpStatusCode.Found)
            {
                HttpError error = await getLayoutResponse.Content.ReadAsAsync<HttpError>();
                this.Logger.DebugFormat(error.Message);

                return null;
            }

            return await getLayoutResponse.Content.ReadAsAsync<Dictionary<string, byte[]>>();
        }
        #endregion
    }
}