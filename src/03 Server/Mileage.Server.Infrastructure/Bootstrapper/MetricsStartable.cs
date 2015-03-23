using Castle.Core;
using Castle.MicroKernel.Registration;
using Metrics;

namespace Mileage.Server.Infrastructure.Bootstrapper
{
    public class MetricsStartable : IStartable
    {
        #region Implementation of IStartable
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (Config.EnableDefaultMetrics.GetValue())
            {
                Metric.Config.WithAllCounters();
            }
        }
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {

        }
        #endregion
    }
}