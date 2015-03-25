using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Mileage.Server.Contracts.Commands
{
    public interface ICommandExecutor
    {
        [NotNull]
        Task<T> Batch<T>([NotNull]Func<ICommandScope, Task<T>> batchAction);
    }
}