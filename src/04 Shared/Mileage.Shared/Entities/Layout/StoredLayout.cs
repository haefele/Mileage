using System.Collections.Generic;

namespace Mileage.Shared.Entities.Layout
{
    public class StoredLayout : AggregateRoot
    {
        public static string CreateId(string userId, string layoutName)
        {
            return string.Format("StoredLayout/{0}/{1}", userId, layoutName);
        }

        public StoredLayout()
        {
            this.LayoutData = new Dictionary<string, byte[]>();
        }

        /// <summary>
        /// Gets or sets the user identifier, from which user this layout is.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the name of the layout.
        /// </summary>
        public string LayoutName { get; set; }
        /// <summary>
        /// Gets or sets the layout data.
        /// </summary>
        public Dictionary<string, byte[]> LayoutData { get; set; } 
    }
}