﻿using CommandLine;
using FQueue.Configuration;
using JetBrains.Annotations;

namespace FQueueNode
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NodeArguments : CommandLineArguments
    {
        [Option('u', URI, HelpText = "Endpoint of Synchronizer API.", Required = true)]
        public string SynchronizerEndpoint
        {
            get => ConfigurationUri;
            set => ConfigurationUri = value;
        }
    }
}