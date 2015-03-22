using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Database.Config;
using Raven.Server;

namespace Mileage.Tests.RavenDBConcurrency
{
    public class Data
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var ravenDbServer = new RavenDbServer(new RavenConfiguration
            {
                RunInMemory = true
            });
            ravenDbServer.Initialize();

            WorkWithDocumentStore(ravenDbServer.DocumentStore).Wait();
        }

        private static async Task WorkWithDocumentStore(IDocumentStore documentStore)
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(new Data
                {
                    Id = "Data/1",
                    Name = "asdf"
                });
                await session.SaveChangesAsync();
            }
            using (var session = documentStore.OpenAsyncSession())
            {
                await session.StoreAsync(new Data
                {
                    Id = "Data/1",
                    Name = "jklö"
                });
                await session.SaveChangesAsync();
            }
            using (var session = documentStore.OpenAsyncSession())
            {
                var data = await session.LoadAsync<Data>("Data/1");

            }
        }
    }
}
