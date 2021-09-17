using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arbor.App.Extensions.Configuration;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.Logging
{
    [Urn(LoggingConstants.SerilogBaseUrn)]
    [UsedImplicitly]
    public class SerilogConfiguration : IConfigurationValues, IValidatableObject
    {
        public SerilogConfiguration(string seqUrl,
            string rollingLogFilePath,
            bool seqEnabled = false,
            bool rollingLogFilePathEnabled = false,
            bool consoleEnabled = false,
            bool debugConsoleEnabled = false)
        {
            Uri? uri = null;

            if (seqEnabled && Uri.TryCreate(seqUrl, UriKind.Absolute, out var foundUri))
            {
                uri = foundUri;
            }

            SeqUrl = uri;
            RollingLogFilePath = rollingLogFilePath;
            SeqEnabled = seqEnabled;
            RollingLogFilePathEnabled = rollingLogFilePathEnabled;
            ConsoleEnabled = consoleEnabled;
            DebugConsoleEnabled = debugConsoleEnabled;
        }

        public bool SeqEnabled { get; }

        public bool RollingLogFilePathEnabled { get; }

        [PublicAPI]
        public bool ConsoleEnabled { get; }

        public bool DebugConsoleEnabled { get; }

        public Uri? SeqUrl { get; }

        public string? RollingLogFilePath { get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SeqEnabled && SeqUrl is null)
            {
                yield return new ValidationResult("Seq is enabled but the url is not set or it is invalid");
            }
        }

        public override string ToString() =>
            $"{nameof(SeqEnabled)}: {SeqEnabled}, {nameof(RollingLogFilePathEnabled)}: {RollingLogFilePathEnabled}, {nameof(ConsoleEnabled)}: {ConsoleEnabled}, {nameof(DebugConsoleEnabled)}: {DebugConsoleEnabled}, {nameof(SeqUrl)}: {SeqUrl}, {nameof(RollingLogFilePath)}: {RollingLogFilePath}";
    }
}