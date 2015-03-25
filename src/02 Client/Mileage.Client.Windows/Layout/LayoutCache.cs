using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Castle.Core.Logging;
using Mileage.Client.Windows.Events;

namespace Mileage.Client.Windows.Layout
{
    public class LayoutCache : IHandle<UserLoggedOutEvent>
    {
        #region Fields
        private readonly Dictionary<string, Dictionary<string, byte[]>> _cache;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCache"/> class.
        /// </summary>
        public LayoutCache()
        {
            this._cache = new Dictionary<string, Dictionary<string, byte[]>>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns whether the specified layout has changed.
        /// </summary>
        /// <param name="layoutName">Name of the layout.</param>
        /// <param name="layoutData">The layout data.</param>
        public bool HasChanged(string layoutName, Dictionary<string, byte[]> layoutData)
        {
            if (this._cache.ContainsKey(layoutName) == false)
            {
                this.Logger.DebugFormat("The layout {0} has changed, we dont have it in our cache.", layoutName);

                this._cache.Add(layoutName, layoutData);
                return true;
            }

            this.Logger.DebugFormat("The layout {0} is in the cache, lets check if it has changed.", layoutName);

            Dictionary<string, byte[]> oldValue = this._cache[layoutName];
            this._cache[layoutName] = layoutData;

            var comparer = new LayoutComparer();
            var hasChanged = comparer.Equals(oldValue, layoutData) == false;

            this.Logger.DebugFormat("The layout {0} has changed {1}.", layoutName, hasChanged);

            return hasChanged;
        }
        /// <summary>
        /// Returns the specified layout.
        /// </summary>
        /// <param name="layoutName">Name of the layout.</param>
        public Dictionary<string, byte[]> Get(string layoutName)
        {
            if (this._cache.ContainsKey(layoutName))
            {
                this.Logger.DebugFormat("Returning the layout {0} from the cache.");

                return this._cache[layoutName];
            }

            return null;
        }
        #endregion

        #region Implementation of IHandle<UserLoggedOutEvent>
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void IHandle<UserLoggedOutEvent>.Handle(UserLoggedOutEvent message)
        {
            this.Logger.DebugFormat("Cleaning the cache because the user logged out.");
            this._cache.Clear();
        }
        #endregion

        #region Internal
        private class LayoutComparer : IEqualityComparer<Dictionary<string, byte[]>>
        {
            public bool Equals(Dictionary<string, byte[]> x, Dictionary<string, byte[]> y)
            {
                if (x.Keys.SequenceEqual(y.Keys) == false)
                    return false;

                if (x.Values.All(f => y.Values.Any(f.SequenceEqual)) == false)
                    return false;

                return true;
            }

            public int GetHashCode(Dictionary<string, byte[]> obj)
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion
    }
}