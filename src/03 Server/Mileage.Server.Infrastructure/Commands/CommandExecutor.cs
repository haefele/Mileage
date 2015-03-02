using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IWindsorContainer _container;

        public CommandExecutor(IWindsorContainer container)
        {
            Guard.AgainstNullArgument("container", container);

            this._container = container;
        }

        public async Task<T> Batch<T>(Func<ICommandScope, Task<T>> batchAction)
        {
            using (this._container.BeginScope())
            using (var documentSession = this._container.Resolve<IAsyncDocumentSession>())
            using (var filesSession = this._container.Resolve<IAsyncFilesSession>())
            {
                var scope = this._container.Resolve<ICommandScope>();

                T result = await batchAction(scope);

                await documentSession.SaveChangesAsync();
                await filesSession.SaveChangesAsync();

                return result;
            }
        }
    }
}