using Arbor.AppModel.ExtensionMethods;
using Serilog.Events;

namespace Arbor.AppModel.Logging
{
    public static class LogEventLevelExtensions
    {
        public static LogEventLevel ParseOrDefault(this string? levelText,
            LogEventLevel level = LogEventLevel.Information)
        {
            if (string.IsNullOrWhiteSpace(levelText))
            {
                return level;
            }

            if (!levelText.TryParse<LogEventLevel>(out var parsedLevel))
            {
                return level;
            }

            return parsedLevel;
        }
    }
}