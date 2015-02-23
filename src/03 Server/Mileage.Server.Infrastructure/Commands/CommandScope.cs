using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Windsor;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Commands
{
    public class CommandScope : ICommandScope
    {
        private readonly IWindsorContainer _container;

        public CommandScope(IWindsorContainer container)
        {
            _container = container;
        }

        public Task<Result<TResult>> Execute<TResult>(ICommand<TResult> command)
        {
            Type handlerType = typeof(CommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic actualCommandHandler = this._container.Resolve(handlerType);

            var wrapper = new CommandHandlerWrapper<TResult>(this, actualCommandHandler);
            return wrapper.Execute(command);
        }

        private class CommandHandlerWrapper<TResult>
        {
            private readonly ICommandScope _scope;
            private readonly object _actualCommandHandler;

            public CommandHandlerWrapper(ICommandScope scope, object actualCommandHandler)
            {
                this._scope = scope;
                this._actualCommandHandler = actualCommandHandler;
            }

            public Task<Result<TResult>> Execute(ICommand<TResult> command)
            {
                MethodInfo methodInfo = this._actualCommandHandler.GetType().GetMethod("Execute");
                return (Task<Result<TResult>>)methodInfo.Invoke(this._actualCommandHandler, new object[] {command, this._scope});
            }
        }
    }
}