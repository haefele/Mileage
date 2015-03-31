using System;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Document;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class AsyncDocumentQueryExtensions
    {
        public static IAsyncDocumentQuery<TResult> WhereEquals<TResult, TValue, T>(this IAsyncDocumentQuery<TResult> self, Expression<Func<T, TValue>> propertySelector, TValue value)
        {
            AbstractDocumentQuery<TResult, AsyncDocumentQuery<TResult>> query = (AsyncDocumentQuery<TResult>)self;
            query.WhereEquals(query.GetMemberQueryPath(propertySelector.Body), value);

            return self;
        }

        public static IAsyncDocumentQuery<TResult> Search<TResult, TValue, T>(this IAsyncDocumentQuery<TResult> self, Expression<Func<T, TValue>> propertySelector, string searchTerms, EscapeQueryOptions escapeQueryOptions = EscapeQueryOptions.RawQuery)
        {
            AbstractDocumentQuery<TResult, AsyncDocumentQuery<TResult>> query = (AsyncDocumentQuery<TResult>)self;
            query.Search(query.GetMemberQueryPath(propertySelector.Body), searchTerms, escapeQueryOptions);

            return self;
        }
    }
}