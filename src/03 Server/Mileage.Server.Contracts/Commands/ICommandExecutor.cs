using System;
using System.Threading.Tasks;

namespace Mileage.Server.Contracts.Commands
{
    public interface ICommandExecutor
    {
        Task<T> Batch<T>(Func<ICommandScope, Task<T>> batchAction);
    }
}