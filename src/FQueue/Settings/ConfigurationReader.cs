using System.IO;
using log4net;
using Newtonsoft.Json;

namespace FQueue.Settings
{
    /// <summary>
    /// Default implementation of <see cref="IConfigurationReader"/>.
    /// Reads implementation from appsettings.json that is in the current working folder.
    /// </summary>
    public class ConfigurationReader : IConfigurationReader
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(ConfigurationReader));

        private static readonly object _lockObject = new object();
        private volatile FQueueConfiguration _fQueueConfiguration;

        private readonly string _configurationFilename;

        public FQueueConfiguration Configuration
        {
            get
            {
                if (this._fQueueConfiguration != null)
                {
                    return this._fQueueConfiguration;
                }

                lock (_lockObject)
                {
                    if (this._fQueueConfiguration != null)
                    {
                        return this._fQueueConfiguration;
                    }

                    _log.Info($"Reading configuration from {_configurationFilename}");

                    string jsonText = File.ReadAllText(_configurationFilename);
                    this._fQueueConfiguration = JsonConvert.DeserializeObject<FQueueConfiguration>(jsonText);
                }

                return this._fQueueConfiguration;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationReader"/> class.
        /// </summary>
        /// <param name="configurationFilename">File to read configuration from.</param>
        public ConfigurationReader(string configurationFilename)
        {
            this._configurationFilename = configurationFilename;
        }
    }
}