﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arbor.AppModel.DependencyInjection;
using Arbor.AppModel.ExtensionMethods;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Messaging
{
    public static class MediatorRegistrationHelper
    {
        public static IServiceCollection Register(IServiceCollection builder,
            IReadOnlyCollection<Assembly> assemblies,
            IModule? module = null)
        {
            ArgumentNullException.ThrowIfNull(builder);

            ArgumentNullException.ThrowIfNull(assemblies);

            var concreteTypes = assemblies.SelectMany(assembly => assembly.GetLoadableTypes())
                                          .Where(type => type.IsPublic && type.IsConcrete()).ToArray();

            RegisterTypes(typeof(IRequestHandler<>), builder, ServiceLifetime.Singleton, concreteTypes);
            RegisterTypes(typeof(IRequestHandler<,>), builder, ServiceLifetime.Singleton, concreteTypes);
            RegisterTypes(typeof(IPipelineBehavior<,>), builder, ServiceLifetime.Singleton, concreteTypes);
            RegisterTypes(typeof(IRequestPostProcessor<,>), builder, ServiceLifetime.Singleton, concreteTypes);
            RegisterTypes(typeof(IRequestPreProcessor<>), builder, ServiceLifetime.Singleton, concreteTypes);
            RegisterTypes(typeof(INotificationHandler<>), builder, ServiceLifetime.Singleton, concreteTypes);

            builder.AddSingleton<IMediator, Mediator>(module);

            builder.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>), module);

            builder.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>), module);

            return builder;
        }

        private static void RegisterTypes(Type openGenericType,
            IServiceCollection builder,
            ServiceLifetime serviceLifetime,
            Type[] concreteTypes,
            IModule? module = null)
        {
            var types = concreteTypes.Where(concreteType => concreteType.Closes(openGenericType)).ToArray();

            foreach (var implementationType in types)
            {
                bool isRegistered = builder.Any(descriptor => descriptor.ImplementationType == implementationType);

                if (!isRegistered)
                {
                    builder.Add(new ExtendedServiceDescriptor(implementationType,
                        implementationType,
                        serviceLifetime,
                        module?.GetType()));
                }

                var interfaces = implementationType.GetInterfaces()
                                                   .Where(@interface => @interface.Closes(openGenericType));

                foreach (var @interface in interfaces)
                {
                    builder.Add(new ExtendedServiceDescriptor(@interface,
                        context => context.GetRequiredService(implementationType),
                        serviceLifetime,
                        module?.GetType()));
                }
            }
        }
    }
}