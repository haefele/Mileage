using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using LiteGuard;
using Mileage.Client.Contracts.Storage;

namespace Mileage.Client.Windows.Storage
{
    public class LiteDatabaseStorage : IDataStorage, IDisposable
    {
        #region Fields
        private readonly LiteEngine _engine;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LiteDatabaseStorage"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public LiteDatabaseStorage(string filePath)
        {
            Guard.AgainstNullArgument("filePath", filePath);

            this._engine = new LiteEngine(filePath);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Stores the specified <paramref name="instance"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be stored.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="instance">The instance.</param>
        public void Store<T>(string id, T instance) 
            where T : new()
        {
            this._engine.BeginTrans();

            Collection<DataWrapper<T>> collection = GetCollection<T>();

            DataWrapper<T> existing = collection.FindById(id);
            if (existing != null)
            {
                collection.Delete(existing);
            }

            collection.Insert(new DataWrapper<T> { Id = id, Data = instance});

            this._engine.Commit();
        }
        /// <summary>
        /// Returns the <typeparamref name="T"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be retrieved.</typeparam>
        /// <param name="id">The identifier.</param>
        public T Get<T>(string id) 
            where T : new()
        {
            Collection<DataWrapper<T>> collection = this.GetCollection<T>();

            DataWrapper<T> existing = collection.FindById(id);
            if (existing != null)
                return existing.Data;

            return default(T);
        }
        /// <summary>
        /// Returns all instances of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be retrieved.</typeparam>
        public IEnumerable<T> GetAll<T>()
            where T : new()
        {
            Collection<DataWrapper<T>> collection = this.GetCollection<T>();
            return collection.All().Select(f => f.Data);
        }
        /// <summary>
        /// Removes the <typeparamref name="T"/> with the specified <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to be deleted.</typeparam>
        /// <param name="id">The identifier.</param>
        public bool Remove<T>(string id) 
            where T : new()
        {
            Collection<DataWrapper<T>> collection = this.GetCollection<T>();
            return collection.Delete(id);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._engine.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the <see cref="Collection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of data in the collection.</typeparam>
        private Collection<DataWrapper<T>> GetCollection<T>() where T : new()
        {
            return this._engine.GetCollection<DataWrapper<T>>(typeof(DataWrapper<T>).FullName);
        }
        #endregion

        #region Internal
        /// <summary>
        /// A wrapper class so I can store the <see cref="Id"/> with the associated <see cref="Data"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class DataWrapper<T>
            where T : new()
        {
            public string Id { get; set; }
            public T Data { get; set; }
        }
        #endregion
    }
}