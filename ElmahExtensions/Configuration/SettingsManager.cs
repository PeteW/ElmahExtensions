using System;
using System.Configuration;

namespace ElmahExtensions.Configuration
{
    public class SettingsManager
    {
        private static CustomErrorHandlerConfiguration _config = null;
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static CustomErrorHandlerConfiguration Config
        {
            get
            {
                if (_config == null)
                    _config = ConfigurationManager.GetSection("ElmahExtensions") as CustomErrorHandlerConfiguration;
                return _config;
            }
        }
    }
    public class CustomErrorHandlerException:Exception{public CustomErrorHandlerException(string message):base(message){}}
}