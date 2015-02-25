using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class ResetIndexesCommand : ICommand<object>
    {
         
    }

    public class ResetIndexesCommandHandler : CommandHandler<ResetIndexesCommand, object>
    {
        private readonly IDocumentStore _documentStore;

        public ResetIndexesCommandHandler(IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument("documentStore", documentStore);

            this._documentStore = documentStore;
        }

        public override Task<Result<object>> Execute(ResetIndexesCommand command, ICommandScope scope)
        {
            return Result.CreateAsync(async () =>
            {
                var compositionContainer = new CompositionContainer(new AssemblyCatalog(this.GetType().Assembly));
                foreach (AbstractIndexCreationTask index in compositionContainer.GetExportedValues<AbstractIndexCreationTask>())
                {
                    await this._documentStore.AsyncDatabaseCommands.ResetIndexAsync(index.IndexName);
                }

                return new object();
            });
        }
    }
}