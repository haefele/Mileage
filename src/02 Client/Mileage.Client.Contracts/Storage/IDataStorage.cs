using System.Collections.Generic;

namespace Mileage.Client.Contracts.Storage
{
    public interface IDataStorage
    {
        /// <summary>
        /// Stores the specified <paramref name="instance"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be stored.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="instance">The instance.</param>
        void Store<T>(string id, T instance) where T : new();

        /// <summary>
        /// Returns the <typeparamref name="T"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be retrieved.</typeparam>
        /// <param name="id">The identifier.</param>
        T Get<T>(string id) where T : new();
        /// <summary>
        /// Returns all instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be retrieved.</typeparam>
        IEnumerable<T> GetAll<T>() where T : new();

        /// <summary>
        /// Removes the <typeparamref name="T"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be deleted.</typeparam>
        /// <param name="id">The identifier.</param>
        bool Remove<T>(string id) where T : new();
    }
}