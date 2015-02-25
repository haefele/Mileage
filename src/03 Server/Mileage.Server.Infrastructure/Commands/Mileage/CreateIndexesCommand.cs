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
        private readonly IDocumentStore _documentStore;

        public CreateIndexesCommandHandler(IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument("documentStore", documentStore);

            this._documentStore = documentStore;
        }

        public override Task<Result<object>> Execute(CreateIndexesCommand command, ICommandScope scope)
        {
            return Result.CreateAsync(async () =>
            {
                await IndexCreation.CreateIndexesAsync(this.GetType().Assembly, this._documentStore);
                return new object();
            });
        }
    }
}