using System;

namespace Tibres
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string sectionName, string key)
            : base($"No configuration setting found for key '{sectionName}:{key}'.")
        {
        }
    }
}
