using CommandLine;
using JetBrains.Annotations;

namespace FQueue.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class CommandLineArguments
    {
        private const string CONFIGURATION = "configuration";

        [Option('c', CONFIGURATION, HelpText = "Configuration file. If not specified, default will be taken.", Required = false)]
        public string ConfigurationFilePath { get; }
    }
}