using System;

namespace Arbor.AppModel.ExtensionMethods
{
    public static class EnumExtensions
    {
        public static bool TryParse<T>(this string value, out T item) where T : Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                item = default!;
                return false;
            }

            if (!Enum.TryParse(typeof(T), value, true, out object? result))
            {
                item = default!;
                return false;
            }

            if (result is not T instance)
            {
                item = default!;
                return false;
            }

            if (!Enum.IsDefined(typeof(T), instance))
            {
                item = default!;
                return false;
            }

            item = instance;
            return true;
        }
    }
}