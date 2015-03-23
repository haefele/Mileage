using System;
using System.Linq;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Hosting;
using Mileage.Server.Infrastructure;
using Raven.Abstractions.Extensions;

namespace Mileage.Server.Hosts.Service
{
    public class MileageService
    {
        #region Fields
        private IDisposable _webApp;
        #endregion

        #region Methods
        /// <summary>
        /// Starts the mileage service.
        /// </summary>
        public void Start()
        {
            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(Config.Addresses.GetValue().Select(f => f.ToString()));

            this._webApp = WebApp.Start<Startup>(startOptions);
        }
        /// <summary>
        /// Stops the mileage service.
        /// </summary>
        public void Stop()
        {
            if (this._webApp != null)
                this._webApp.Dispose();
        }
        #endregion
    }
}