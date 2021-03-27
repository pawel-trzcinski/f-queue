using CommandLine;
using FQueue.Configuration;
using JetBrains.Annotations;

namespace FQueueSynchronizer
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SynchronizerArguments : CommandLineArguments
    {
        [Option('u', URI, HelpText = "Etcd address", Required = true)]
        public string EtcdEndpoint
        {
            get => ConfigurationUri;
            set => ConfigurationUri = value;
        }
    }
}