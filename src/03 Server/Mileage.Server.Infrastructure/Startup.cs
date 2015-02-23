using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.Core.Configuration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using Microsoft.Owin.Cors;
using Mileage.Server.Infrastructure.Api.Configuration;
using Mileage.Server.Infrastructure.Api.Controllers;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Api.MessageHandlers;
using Mileage.Server.Infrastructure.Windsor;
using Newtonsoft.Json;
using Owin;
using Raven.Client;

namespace Mileage.Server.Infrastructure
{
    public class Startup
    {
        #region Methods
        /// <summary>
        /// Configurations the OWIN webservice host.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void Configuration(IAppBuilder app)
        {
            this.UseCors(app);
            this.UseWebApi(app);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Makes the app use cors.
        /// </summary>
        /// <param name="app">The application.</param>
        private void UseCors(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
        }
        /// <summary>
        /// Makes the app use webapi.
        /// </summary>
        /// <param name="app">The application.</param>
        private void UseWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            this.ConfigureWindsor(config);
            this.ConfigureFilters(config);
            this.ConfigureMessageHandlers(config);
            this.ConfigureWebApiAssemblies(config);
            this.ConfigureRoutes(config);
            this.ConfigureAllowOnlyJson(config);

            app.UseWebApi(config);
        }
        /// <summary>
        /// Configures the castle windsor container.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureWindsor(HttpConfiguration config)
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            
            config.DependencyResolver = new WindsorResolver(container);
        }
        /// <summary>
        /// Configures the default filters.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureFilters(HttpConfiguration config)
        {
        }
        /// <summary>
        /// Configures the message handlers.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureMessageHandlers(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new RequestExecutionTimeMessageHandler());
            config.MessageHandlers.Add(new ConcurrentRequestCountMessageHandler());

            if (bool.Parse(Dependency.OnAppSettingsValue("Mileage/CompressResponses").Value))
            {
                config.MessageHandlers.Add(new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
            }

            config.MessageHandlers.Add(new LocalizationMessageHandler());

            if (bool.Parse(Dependency.OnAppSettingsValue("Mileage/EnableDebugRequestResponseLogging").Value))
            {
                config.MessageHandlers.Add(new LoggingMessageHandler());
            }

            config.MessageHandlers.Add(new VersionValidationMessageHandler());
            config.MessageHandlers.Add(new LicenseValidationMessageHandler());
        }
        /// <summary>
        /// Configures the assemblies that contain webapi controllers.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureWebApiAssemblies(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IAssembliesResolver), new MileageAssembliesResolver());
        }
        /// <summary>
        /// Configures the routes.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
        /// <summary>
        /// Configures the config to allow only json requests.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureAllowOnlyJson(HttpConfiguration config)
        {
            if (bool.Parse(Dependency.OnAppSettingsValue("Mileage/FormatResponses").Value))
            {
                config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            }

            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(config.Formatters.JsonFormatter));
        }
        #endregion
    }
}