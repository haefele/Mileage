using System;

namespace Mileage.Shared.Results
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Returns the <see cref="Result.Message"/> if available.
        /// Otherwise the <paramref name="defaultMessage"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="defaultMessage">The default message.</param>
        public static string MessageOr(this Result result, string defaultMessage)
        {
            if (string.IsNullOrWhiteSpace(defaultMessage))
                throw new ArgumentNullException("defaultMessage");

            return result.Message ?? defaultMessage;
        }
        /// <summary>
        /// Returns the <see cref="Result{T}.Message"/> if available.
        /// Otherwise the <paramref name="defaultMessage"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="defaultMessage">The default message.</param>
        public static string MessageOr<T>(this Result<T> result, string defaultMessage)
        {
            if (string.IsNullOrWhiteSpace(defaultMessage))
                throw new ArgumentNullException("defaultMessage");

            return result.Message ?? defaultMessage;
        }
        /// <summary>
        /// Executes the specified <paramref name="action"/> if <see cref="Result.IsSuccess"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="action">The action.</param>
        public static void IfSuccess(this Result result, Action action)
        {
            if (result.IsSuccess)
            {
                action();
            }
        }
        /// <summary>
        /// Executes the specified <paramref name="action"/> if <see cref="Result{T}.IsSuccess"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="action">The action.</param>
        public static void IfSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Data);
            }
        }
        /// <summary>
        /// Returns the <see cref="Result{T}.Data"/> if <see cref="Result{T}.IsSuccess"/> otherwise the <paramref name="defaultValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of data the result can contain.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="defaultValue">The default value.</param>
        public static T DataOr<T>(this Result<T> result, T defaultValue)
        {
            if (result.IsSuccess)
                return result.Data;

            return defaultValue;
        }
    }
}