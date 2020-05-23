using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.ExtensionMethods
{
    [PublicAPI]
    public static class NullExtensions
    {
        public static bool HasValue<T>([NotNullWhen(true)] this T? item) where T : class => item is {};

        public static bool IsDefault<T>(this T item) where T : struct => !Equals(item, default(T));
    }
}