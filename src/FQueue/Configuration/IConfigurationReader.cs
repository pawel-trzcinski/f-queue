namespace FQueue.Configuration
{
    /// <summary>
    /// Interface for custom reading appsettings.josn configuration.
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Reads configuration from appsettings.json file.
        /// </summary>
        /// <returns>Configuration of the FQueue.</returns>
        FQueueConfiguration Configuration { get; }
    }
}
