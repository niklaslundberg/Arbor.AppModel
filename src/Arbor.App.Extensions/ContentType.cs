using System;

namespace Arbor.App.Extensions
{
    public class ContentType
    {
        public static readonly ContentType Json = new("JSON", "application/json");

        public ContentType(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }

        public static bool IsJson(string? contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            if (ReferenceEquals(contentType, Json.Value))
            {
                return true;
            }

            if (string.Equals(contentType, Json.Value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (contentType.StartsWith("application/", StringComparison.OrdinalIgnoreCase) &&
                contentType.EndsWith("+json", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}