﻿using System;
using System.Collections.Generic;
using System.Linq;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;

namespace Arbor.AppModel.Cli
{
    public static class ParameterParser
    {
        public static string? ParseParameter(this IReadOnlyCollection<string> parameters,
            string parameterName)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(parameterName));
            }

            string trimmedName = parameterName.Trim();
            string prefix = $"{trimmedName}=";

            string[] matchingArgs = parameters.NotNull().Select(param => param.Trim())
                                              .Where(param =>
                                                   param.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                                              .ToArray();

            if (matchingArgs.Length == 0)
            {
                return null;
            }

            if (matchingArgs.Length > 1)
            {
                throw new InvalidOperationException($"Found more than 1 parameter named '{parameterName}'");
            }

            string value = matchingArgs[0][prefix.Length..].Trim();

            return value;
        }
    }
}