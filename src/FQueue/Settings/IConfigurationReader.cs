namespace FQueue.Settings
{
    /// <summary>
    /// Interface for custom reading appsettings.josn configuration.
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Reads configuration from appsettings.json file.
        /// </summary>
        /// <returns>Bussiness configuration of the RandomSimulation.</returns>
        FQueueConfiguration Configuration { get; }
    }
}
