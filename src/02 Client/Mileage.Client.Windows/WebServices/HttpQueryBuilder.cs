using System.Text;

namespace Mileage.Client.Windows.WebServices
{
    /// <summary>
    /// A builder class for the HTTP query string.
    /// </summary>
    public class HttpQueryBuilder
    {
        private readonly StringBuilder _queryStringBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpQueryBuilder"/> class.
        /// </summary>
        public HttpQueryBuilder()
        {
            this._queryStringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Adds the specified <paramref name="key"/> and its <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddParameter(string key, object value)
        {
            bool firstParameter = this._queryStringBuilder.Length == 0;
            this._queryStringBuilder.Append(firstParameter ? "?" : "&");

            this._queryStringBuilder.AppendFormat("{0}={1}", key, value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// This is the HTTP query string.
        /// </summary>
        public override string ToString()
        {
            return this._queryStringBuilder.ToString();
        }
    }
}