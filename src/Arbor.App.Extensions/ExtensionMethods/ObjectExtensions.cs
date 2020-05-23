namespace Arbor.App.Extensions.ExtensionMethods
{
    public static class ObjectExtensions
    {
        public static bool HasValue<T>(this T item) where T : class => item is {};
    }
}