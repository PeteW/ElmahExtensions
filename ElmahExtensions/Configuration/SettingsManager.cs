using System;
using System.Configuration;

namespace ElmahExtensions.Configuration
{
    public class SettingsManager
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static CustomErrorHandlerConfiguration Config
        {
            get { return ConfigurationManager.GetSection("ElmahExtensions") as CustomErrorHandlerConfiguration; }
        }
    }
    public class CustomErrorHandlerException:Exception{public CustomErrorHandlerException(string message):base(message){}}
}