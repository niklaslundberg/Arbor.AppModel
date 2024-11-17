namespace Arbor.AppModel.Configuration;

public class ConfigurationError(string error)
{
    public string Error { get; } = error;
}