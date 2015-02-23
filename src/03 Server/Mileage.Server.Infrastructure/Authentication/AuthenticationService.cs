using System;
using System.Collections.Generic;
using System.Linq;
using LiteGuard;
using Microsoft.Owin;
using Mileage.Localization.Server.Authentication;
using Mileage.Server.Contracts.Authentication;
using Mileage.Shared.Entities;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Results;
using Raven.Abstractions.Data;
using Raven.Client;

namespace Mileage.Server.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly IDocumentStore _documentStore;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="documentStore">The document store.</param>
        public AuthenticationService(IDocumentStore documentStore)
        {
            Guard.AgainstNullArgument("documentStore", documentStore);

            this._documentStore = documentStore;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the authenticated user id from the specified <paramref name="requestContext"/>.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        public Result<string> GetAuthenticatedUserId(IOwinContext requestContext)
        {
            Guard.AgainstNullArgument("requestContext", requestContext);

            Result<string> authenticationToken = this.GetToken(requestContext);

            if (authenticationToken.IsError)
                return authenticationToken;

            using (this._documentStore.AggressivelyCache())
            using (IDocumentSession session = this._documentStore.OpenSession())
            {
                var token = session.Load<AuthenticationToken>(AuthenticationToken.CreateId(authenticationToken.Data));

                if (token == null)
                    return Result.AsError(AuthenticationMessages.NoAuthenticationTokenGiven);

                if (token.IsValid() == false)
                    return Result.AsError(AuthenticationMessages.AuthenticationTokenExpired);

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

            return Result.AsError(AuthenticationMessages.NoAuthenticationTokenGiven);
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

            return Result.AsError(AuthenticationMessages.NoAuthenticationTokenGiven);
        }
        /// <summary>
        /// Tries to extract the <see cref="AuthenticationToken.Token"/> from the request headers.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        private Result<string> GetTokenFromHeader(IOwinContext requestContext)
        {
            string authorizationHeader = requestContext.Request.Headers.Get("Authorization");

            string[] parts = authorizationHeader.Split(' ');

            if (parts.Length != 2)
                return Result.AsError(AuthenticationMessages.NoAuthenticationTokenGiven);

            if (parts[1].Equals("Mileage", StringComparison.InvariantCultureIgnoreCase))
                return Result.AsError(AuthenticationMessages.NoAuthenticationTokenGiven);

            return Result.AsSuccess(parts[2]);
        }
        #endregion
    }
}