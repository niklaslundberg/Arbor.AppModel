using System;

namespace Arbor.AppModel;

public class ContentType(string name, string value)
{
    public static readonly ContentType Json = new("JSON", "application/json");

    public string Name { get; } = name;

    public string Value { get; } = value;

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