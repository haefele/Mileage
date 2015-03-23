using System.Linq;
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
            startOptions.Urls.AddRange(Config.Addresses.GetValue().Select(f => f.ToString()));

            using (WebApp.Start<Startup>(startOptions))
            {
                System.Console.WriteLine("Mileage Web-API started.");
                System.Console.WriteLine("Press <Enter> to close.");

                System.Console.ReadLine();
            }
        }
    }
}
