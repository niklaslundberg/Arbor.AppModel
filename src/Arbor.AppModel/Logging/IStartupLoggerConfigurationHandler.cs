using Serilog;

namespace Arbor.AppModel.Logging
{
    public interface IStartupLoggerConfigurationHandler
    {
        LoggerConfiguration Handle(LoggerConfiguration loggerConfiguration);
    }
}