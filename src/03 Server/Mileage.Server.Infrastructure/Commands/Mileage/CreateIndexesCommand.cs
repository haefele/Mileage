using System;
using System.Threading.Tasks;
using System.Windows.Input;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Commands.Mileage
{
    public class CreateIndexesCommand : ICommand<object>
    {
         
    }

    public class CreateIndexesCommandHandler : CommandHandler<CreateIndexesCommand, object>
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIndexesCommandHandler"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public CreateIndexesCommandHandler(IDocumentStore documentStore)
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
        public override Task<Result<object>> Execute(CreateIndexesCommand command, ICommandScope scope)
        {
            Guard.AgainstNullArgument("command", command);
            Guard.AgainstNullArgument("scope", scope);

            return Result.CreateAsync(async () =>
            {
                await IndexCreation.CreateIndexesAsync(this.GetType().Assembly, this._documentStore);
                return new object();
            });
        }
        #endregion
    }
}