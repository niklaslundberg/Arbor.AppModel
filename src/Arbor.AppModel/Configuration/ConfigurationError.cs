﻿namespace Arbor.AppModel.Configuration
{
    public class ConfigurationError
    {
        public ConfigurationError(string error) => Error = error;

        public string Error { get; }
    }
}