using System;
using LiteGuard;
using Raven.Database.Bundles.Replication.Data;

namespace Mileage.Server.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string AppendEmailSuffix(this string self, string suffix)
        {
            Guard.AgainstNullArgument("self", self);

            if (self.Contains("@"))
                return self;

            return string.Format("{0}@{1}", self.TryRemoveAtSuffix(), suffix.TryRemoveAtPrefix());
        }

        public static string UpdateEmailSuffix(this string self, string suffix)
        {
            Guard.AgainstNullArgument("self", self);

            int splitIndex = self.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);

            if (splitIndex == -1)
                return self;

            string prefix = self.Substring(0, splitIndex);
            return string.Format("{0}@{1}", prefix, suffix.TryRemoveAtPrefix());
        }

        private static string TryRemoveAtPrefix(this string emailSuffix)
        {
            if (emailSuffix.StartsWith("@"))
                return emailSuffix.Substring(1, emailSuffix.Length - 1);

            return emailSuffix;
        }

        private static string TryRemoveAtSuffix(this string self)
        {
            if (self.EndsWith("@"))
                return self.Substring(0, self.Length - 1);

            return self;
        }
    }
}