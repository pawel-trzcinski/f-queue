namespace FQueue.Settings
{
    public class CommandLineOptions
    {
        public string ConfigurationFilePath { get; }

        private CommandLineOptions(CommandLineArguments commandLineArguments)
        {
            ConfigurationFilePath = commandLineArguments.ConfigurationFilePath;
        }

        public static CommandLineOptions FromCommandLineArguments(CommandLineArguments commandLineArguments)
        {
            return new CommandLineOptions(commandLineArguments);
        }
    }
}