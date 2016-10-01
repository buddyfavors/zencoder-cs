//-----------------------------------------------------------------------
// <copyright file="ZencoderSettings.cs" company="Tasty Codes">
//     Copyright (c) 2010 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace Zencoder
{
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    /// <summary>
    /// Zencoder configuration settings.
    /// </summary>
    public class ZencoderSettings
    {
        private static ZencoderSettings s_settings = new ZencoderSettings();
        private readonly IConfigurationRoot m_ConfigurationRoot = null;

        public ZencoderSettings()
        {
            m_ConfigurationRoot = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
        }

        /// <summary>
        /// Gets current configuration.
        /// </summary>
        public static ZencoderSettings Section
        {
            get { return s_settings; }
        }

        /// <summary>
        /// Gets the default API to use.
        /// </summary>
        public string ApiKey => m_ConfigurationRoot.GetValue<string>("zencoder:apiKey");

        /// <summary>
        /// Gets a collection of named types that implement <see cref="INotificationReceiver"/> that should be 
        /// notifiied when a notification is received.
        /// </summary>
        public IEnumerable<string> Notifications
        {
            get { return m_ConfigurationRoot.GetValue<IEnumerable<string>>("zencoder:notifications") ?? new string[0]; }
        }
    }
}
