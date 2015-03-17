using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Mileage.Server.Hosts.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(f =>
            {
                f.Service<MileageService>(d =>
                {
                    d.ConstructUsing(x => new MileageService());
                    d.WhenStarted(x => x.Start());
                    d.WhenStopped(x => x.Stop());
                });

                f.RunAsLocalSystem();
                f.StartAutomatically();

                f.SetDescription("The Mileage HTTP Api.");
                f.SetDisplayName("Mileage HTTP Api");
                f.SetServiceName("MileageHTTPApi");
            });
        }
    }
}
