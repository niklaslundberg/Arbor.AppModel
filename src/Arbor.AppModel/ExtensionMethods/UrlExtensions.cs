using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.AppModel.ExtensionMethods
{
    public static class UrlExtensions
    {
        [PublicAPI]
        public static string CreateQueryWithQuestionMark(
            this IEnumerable<KeyValuePair<string, string>> parameters) =>
            $"?{CreateQueryWithoutQuestionMark(parameters)}";

        [PublicAPI]
        public static string CreateQueryWithoutQuestionMark(
            this IEnumerable<KeyValuePair<string, string>> parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            string query =
                $"{string.Join("&", parameters.Select(parameter => $"{Uri.EscapeDataString(parameter.Key)}={Uri.EscapeDataString(parameter.Value)}"))}";

            return query;
        }

        [PublicAPI]
        public static Uri WithQueryFromParameters(this Uri uri,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var builder = new UriBuilder(uri) { Query = CreateQueryWithoutQuestionMark(parameters) };

            return builder.Uri;
        }

        public static Uri? ParseUriOrDefault(this string? value)
        {
            if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                return uri;
            }

            return default;
        }
    }
}