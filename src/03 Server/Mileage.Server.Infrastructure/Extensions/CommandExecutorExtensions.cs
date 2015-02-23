using System.Threading.Tasks;
using Metrics.Core;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class CommandExecutorExtensions
    {
        public static Task<Result<TResult>> Execute<TResult>(this ICommandExecutor commandExecutor,
            ICommand<TResult> command)
        {
            return commandExecutor.Batch(f => f.Execute(command));
        }
    }
}