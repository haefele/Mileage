using System.Threading.Tasks;
using Mileage.Shared.Results;

namespace Mileage.Server.Contracts.Commands
{
    public interface ICommandScope
    {
        Task<Result<TResult>> Execute<TResult>(ICommand<TResult> command);
    }
}