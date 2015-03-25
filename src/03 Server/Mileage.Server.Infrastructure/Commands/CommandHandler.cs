using System.Threading.Tasks;
using JetBrains.Annotations;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;
using Raven.Client;
using Raven.Client.FileSystem;

namespace Mileage.Server.Infrastructure.Commands
{
    public abstract class CommandHandler<TCommand, TResult> 
        where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        [NotNull]
        public abstract Task<Result<TResult>> Execute([NotNull]TCommand command, [NotNull]ICommandScope scope);
    }
}