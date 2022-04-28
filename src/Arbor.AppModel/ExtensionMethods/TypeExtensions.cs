﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Arbor.AppModel.Configuration;
using JetBrains.Annotations;

namespace Arbor.AppModel.ExtensionMethods
{
    [PublicAPI]
    public static class TypeExtensions
    {
        public static Type? TryGetType(this ExcludedAutoRegistrationType excluded)
        {
            try
            {
                return Type.GetType(excluded.FullName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ImmutableArray<Type> FindPublicConcreteTypesImplementing<T>(
            [NotNull] this IReadOnlyCollection<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var types = assemblies
                       .Select(assembly => assembly.GetLoadableTypes().Where(IsPublicConcreteTypeImplementing<T>))
                       .SelectMany(assemblyTypes => assemblyTypes).ToImmutableArray();

            return types;
        }

        public static bool TakesTypeInPublicCtor<T>([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var constructorInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if (constructorInfos.Length != 1)
            {
                return false;
            }

            var parameterInfos = constructorInfos[0].GetParameters();

            if (parameterInfos.Length != 1)
            {
                return false;
            }

            return parameterInfos[0].ParameterType == typeof(T);
        }

        public static bool IsPublicConcreteTypeImplementing<T>([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            bool isCorrectType = IsConcreteTypeImplementing<T>(type);

            if (!isCorrectType)
            {
                return false;
            }

            return type.IsPublic;
        }

        public static bool IsConcreteTypeImplementing<T>(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (!type.IsClass)
            {
                return false;
            }

            if (!typeof(T).IsAssignableFrom(type))
            {
                return false;
            }

            return true;
        }

        public static bool IsPublicClassWithDefaultConstructor(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (!type.IsClass)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (!type.IsPublic)
            {
                return false;
            }

            bool isInstantiatable = type.GetConstructor(Type.EmptyTypes) != null;

            return isInstantiatable;
        }

        public static ImmutableArray<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            try
            {
                return assembly.GetTypes().ToImmutableArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.NotNull().ToImmutableArray();
            }
        }

        public static ImmutableArray<Type> GetLoadablePublicConcreteTypeImplementing<T>(this Assembly assembly) =>
            assembly.GetLoadableTypes().Where(t => t.IsPublicConcreteTypeImplementing<T>()).ToImmutableArray();

        public static ImmutableArray<Type>
            GetLoadablePublicConcreteTypesImplementing<T>(this IEnumerable<Assembly> assemblies) => assemblies
           .SelectMany(assembly => assembly.GetLoadablePublicConcreteTypeImplementing<T>()).ToImmutableArray();

        public static bool HasAttribute<T>(this Type type) where T : Attribute => type.GetCustomAttribute<T>() != null;

        // Originally taken from https://github.com/JasperFx/baseline/
        public static bool Closes(this Type? type, [NotNull] Type openType)
        {
            if (openType == null)
            {
                throw new ArgumentNullException(nameof(openType));
            }

            if (type is null)
            {
                return false;
            }

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == openType)
            {
                return true;
            }

            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.Closes(openType))
                {
                    return true;
                }
            }

            var baseType = typeInfo.BaseType;

            if (baseType is null)
            {
                return false;
            }

            var baseTypeInfo = baseType.GetTypeInfo();

            bool closes = baseTypeInfo.IsGenericType && baseType.GetGenericTypeDefinition() == openType;

            if (closes)
            {
                return true;
            }

            return typeInfo.BaseType?.Closes(openType) ?? false;
        }

        // Originally taken from https://github.com/JasperFx/baseline/
        public static bool IsConcrete(this Type? type)
        {
            if (type is null)
            {
                return false;
            }

            var typeInfo = type.GetTypeInfo();

            return !typeInfo.IsAbstract && !typeInfo.IsInterface;
        }
    }
}