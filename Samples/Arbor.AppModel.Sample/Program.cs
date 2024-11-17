using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arbor.AppModel.Application;
using Arbor.AppModel.Configuration;
using Arbor.AppModel.ExtensionMethods;
using Arbor.AppModel.Logging;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Urns;
using Serilog.Events;

namespace Arbor.AppModel.Sample
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine(
                $"Application name: {new InMemoryKeyValueConfiguration([]).GetApplicationName()}");

            var version = ApplicationVersionHelper.GetAppVersion();
            Console.WriteLine($"Application version AssemblyFullName: {version?.AssemblyFullName}");
            Console.WriteLine($"Application version AssemblyVersion: {version?.AssemblyVersion}");
            Console.WriteLine($"Application version FileVersion: {version?.FileVersion}");
            Console.WriteLine($"Application version InformationalVersion: {version?.InformationalVersion}");

            var assemblies = ApplicationAssemblies.FilteredAssemblies(["Arbor"]);

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

            await AppStarter<SampleStartup>.StartAsync(args, new Dictionary<string, string>()
            {
                [ConfigurationConstants.LogLevel] = LogEventLevel.Debug.ToString(),
                [LoggingConstants.MicrosoftLevel] = LogEventLevel.Debug.ToString()
            });
        }
    }
}