using System.Threading.Tasks;
using JetBrains.Annotations;
using Mileage.Shared.Results;

namespace Mileage.Server.Contracts.Commands
{
    public interface ICommandScope
    {
        [NotNull]
        Task<Result<TResult>> Execute<TResult>([NotNull]ICommand<TResult> command);
    }
}