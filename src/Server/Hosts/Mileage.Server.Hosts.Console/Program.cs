using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Hosting;
using Mileage.Server.Infrastructure;
using Raven.Abstractions.Extensions;

namespace Mileage.Server.Hosts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var startOptions = new StartOptions();
            startOptions.Urls.AddRange(Dependency.OnAppSettingsValue("Mileage/Addresses").Value.Split('|'));

            using (WebApp.Start<Startup>(startOptions))
            {
                System.Console.WriteLine("Mileage Web-API started.");
                System.Console.WriteLine("Press <Enter> to close.");

                System.Console.ReadLine();
            }
        }
    }
}
