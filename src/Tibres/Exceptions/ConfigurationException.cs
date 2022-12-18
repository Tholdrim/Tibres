using System;

namespace Tibres
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string key)
            : base($"No configuration setting found for key '{key}'.")
        {
        }
    }
}
