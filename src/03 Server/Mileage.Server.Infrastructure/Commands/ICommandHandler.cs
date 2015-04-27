﻿using System.Threading.Tasks;
using JetBrains.Annotations;
using Mileage.Server.Contracts.Commands;
using Mileage.Shared.Results;

namespace Mileage.Server.Infrastructure.Commands
{
    public interface ICommandHandler<in TCommand, TResult> 
        where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        [NotNull]
        Task<Result<TResult>> Execute([NotNull]TCommand command, [NotNull]ICommandScope scope);
    }
}