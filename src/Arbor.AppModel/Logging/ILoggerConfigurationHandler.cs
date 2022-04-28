using Serilog;

namespace Arbor.AppModel.Logging
{
    public interface ILoggerConfigurationHandler
    {
        LoggerConfiguration Handle(LoggerConfiguration loggerConfiguration);
    }
}