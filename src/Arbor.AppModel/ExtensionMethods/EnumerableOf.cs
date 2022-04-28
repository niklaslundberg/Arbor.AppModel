using System;
using System.Collections.Immutable;
using System.Linq;

namespace Arbor.AppModel.ExtensionMethods
{
    public static class EnumerableOf<T> where T : class
    {
        private static readonly Lazy<ImmutableArray<T>> Lazy = new(Initialize);

        public static ImmutableArray<T> All => Lazy.Value;

        private static ImmutableArray<T> Initialize()
        {
            Type type = typeof(T);

            return type.GetFields()
                       .Where(field => field.FieldType == type && field.IsInitOnly && field.IsStatic && field.IsPublic)
                       .Select(field => (T)field.GetValue(null)!).ToImmutableArray();
        }
    }
}