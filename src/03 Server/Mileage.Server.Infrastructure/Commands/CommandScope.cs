using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Windsor;
using JetBrains.Annotations;
using LiteGuard;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Commands
{
    public class CommandScope : ICommandScope
    {
        #region Fields
        private readonly IWindsorContainer _container;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandScope"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CommandScope([NotNull]IWindsorContainer container)
        {
            Guard.AgainstNullArgument("container", container);

            this._container = container;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="command">The command.</param>
        [NotNull]
        public Task<Result<TResult>> Execute<TResult>([NotNull]ICommand<TResult> command)
        {
            Guard.AgainstNullArgument("command", command);

            Type handlerType = typeof(CommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic actualCommandHandler = this._container.Resolve(handlerType);

            var wrapper = new CommandHandlerWrapper<TResult>(this, actualCommandHandler);
            return wrapper.Execute(command);
        }
        #endregion

        #region Private Methods
        private class CommandHandlerWrapper<TResult>
        {
            private readonly ICommandScope _scope;
            private readonly object _actualCommandHandler;

            public CommandHandlerWrapper([NotNull]ICommandScope scope, [NotNull]object actualCommandHandler)
            {
                Guard.AgainstNullArgument("scope", scope);
                Guard.AgainstNullArgument("actualCommandHandler", actualCommandHandler);

                this._scope = scope;
                this._actualCommandHandler = actualCommandHandler;
            }

            public Task<Result<TResult>> Execute(ICommand<TResult> command)
            {
                Guard.AgainstNullArgument("command", command);

                MethodInfo methodInfo = this._actualCommandHandler.GetType().GetMethod("Execute");
                return (Task<Result<TResult>>)methodInfo.Invoke(this._actualCommandHandler, new object[] {command, this._scope});
            }
        }
        #endregion
    }
}