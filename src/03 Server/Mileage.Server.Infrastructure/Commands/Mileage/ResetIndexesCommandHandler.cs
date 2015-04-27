using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Mileage;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class ResetIndexesCommandHandler : ICommandHandler<ResetIndexesCommand, object>
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetIndexesCommandHandler"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public ResetIndexesCommandHandler(IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument("documentStore", documentStore);

            this._documentStore = documentStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public Task<Result<object>> Execute(ResetIndexesCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument("command", command);
            Guard.AgainstNullArgument("scope", scope);

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
        #endregion
    }
}