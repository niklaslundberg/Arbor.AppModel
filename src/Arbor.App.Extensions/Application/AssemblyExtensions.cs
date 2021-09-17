using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Arbor.App.Extensions.Application
{
    public static class AssemblyExtensions
    {
        public static T LoadReferenceAssemblies<T>(this T assemblies,
            AppDomain? appDomain = null,
            Action<Exception>? exceptionHandler = null) where T : IEnumerable<Assembly>
        {
            var domain = appDomain ?? AppDomain.CurrentDomain;

            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                var referencedAssemblies = assembly.GetReferencedAssemblies();

                foreach (var referencedAssembly in referencedAssemblies)
                {
                    try
                    {
                        if (!domain.GetAssemblies().Any(loaded =>
                            !loaded.IsDynamic && loaded.GetName() == referencedAssembly))
                        {
                            domain.Load(referencedAssembly);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler?.Invoke(ex);
                    }
                }
            }

            return assemblies;
        }
    }
}