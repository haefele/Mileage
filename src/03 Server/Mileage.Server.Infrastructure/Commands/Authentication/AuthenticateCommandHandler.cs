using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LiteGuard;
using Microsoft.Owin;
using Mileage.Localization.Server.Commands;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Contracts.Commands.Authentication;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Commands.Authentication
{
    public class AuthenticateCommandHandler : CommandHandler<AuthenticateCommand, string>
    {
        #region Constants
        public const string AuthenticationMechanism = "Mileage";
        #endregion

        #region Fields
        private readonly IAsyncDocumentSession _documentSession;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateCommandHandler"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public AuthenticateCommandHandler(IAsyncDocumentSession documentSession)
        {
            Guard.AgainstNullArgument("documentSession", documentSession);

            this._documentSession = documentSession;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="scope">The scope.</param>
        public override async Task<Result<string>> Execute(AuthenticateCommand command, ICommandScope scope)
        {
            Result<string> authenticationToken = this.GetToken(command.OwinContext);

            if (authenticationToken.IsError)
                return authenticationToken;

            using (this._documentSession.Advanced.DocumentStore.AggressivelyCache())
            {
                AuthenticationToken token = await this._documentSession.LoadAsync<AuthenticationToken>(AuthenticationToken.CreateId(authenticationToken.Data)).WithCurrentCulture();

                if (token == null)
                    return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);

                if (token.IsValid() == false)
                    return Result.AsError(CommandMessages.AuthenticationTokenExpired);

                return Result.AsSuccess(token.UserId);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Tries to extract the <see cref="AuthenticationToken.Token"/> from the request.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetToken(IOwinContext requestContext)
        {
            Result<string> tokenFromUri = this.GetTokenFromUri(requestContext);

            if (tokenFromUri.IsSuccess)
                return tokenFromUri;

            Result<string> tokenFromHeader = this.GetTokenFromHeader(requestContext);

            if (tokenFromHeader.IsSuccess)
                return tokenFromHeader;

            return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);
        }
        /// <summary>
        /// Tries to extract the <see cref="AuthenticationToken.Token"/> from the request query parameters.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetTokenFromUri(IOwinContext requestContext)
        {
            IList<string> values = requestContext.Request.Query.GetValues("token");

            if (values != null && values.Any())
                return Result.AsSuccess(values.First());

            return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);
        }
        /// <summary>
        /// Tries to extract the <see cref="AuthenticationToken.Token"/> from the request headers.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetTokenFromHeader(IOwinContext requestContext)
        {
            string authorizationHeader = requestContext.Request.Headers.Get("Authorization");

            if (authorizationHeader == null)
                return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);

            string[] parts = authorizationHeader.Split(' ');

            if (parts.Length != 2)
                return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);
            
            if (parts[0].Equals(AuthenticationMechanism, StringComparison.InvariantCultureIgnoreCase) == false)
                return Result.AsError(CommandMessages.NoAuthenticationTokenGiven);

            return Result.AsSuccess(parts[1]);
        }
        #endregion
    }
}