using System.Threading.Tasks;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Commands
{
    public abstract class CommandHandler<TCommand, TResult> 
        where TCommand : ICommand<TResult>
    {
        public abstract Task<Result<TResult>> Execute(TCommand command, ICommandScope scope);
    }
}