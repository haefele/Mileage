using System.Collections.Generic;

namespace Mileage.Shared.Common
{
    public static class ClientIds
    {
        /// <summary>
        /// Returns all available client ids.
        /// </summary>
        public static IEnumerable<string> Get()
        {
            yield return Desktop;
        }

        public static string Desktop = "Mileage-Desktop";
    }
}