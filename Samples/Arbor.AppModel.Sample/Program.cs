using System;
using System.Collections.Specialized;
using System.Linq;
using Arbor.AppModel.Application;
using Arbor.AppModel.ExtensionMethods;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;

namespace Arbor.AppModel.Sample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(
                $"Application name: {new InMemoryKeyValueConfiguration(new NameValueCollection()).GetApplicationName()}");

            var version = ApplicationVersionHelper.GetAppVersion();
            Console.WriteLine($"Application version AssemblyFullName: {version?.AssemblyFullName}");
            Console.WriteLine($"Application version AssemblyVersion: {version?.AssemblyVersion}");
            Console.WriteLine($"Application version FileVersion: {version?.FileVersion}");
            Console.WriteLine($"Application version InformationalVersion: {version?.InformationalVersion}");

            var assemblies = ApplicationAssemblies.FilteredAssemblies(new[] { "Arbor" });

            ApplicationAssemblies.FilteredAssemblies().LoadReferenceAssemblies();

            static void Handle(Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Loading types from assemblies " +
                              assemblies.Where(a => !a.IsDynamic).AsString(a => a.GetName().Name));

            try
            {
                var urnTypes = UrnTypes.GetUrnTypesInAssemblies(Handle, assemblies.ToArray());

                if (urnTypes.IsEmpty)
                {
                    Console.WriteLine("Found no types");
                }
                else
                {
                    foreach (var urnTypeMapping in urnTypes)
                    {
                        Console.WriteLine("Found URN type" + urnTypeMapping);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not get URN types from assemblies " +
                                  assemblies.Where(a => !a.IsDynamic).AsString(a => a.GetName().Name));

                Console.WriteLine(e);
            }
        }
    }
}