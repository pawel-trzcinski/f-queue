using System;
using JetBrains.Annotations;
using log4net;

namespace FQueue.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class CommandLineArguments
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CommandLineArguments));

        protected const string URI = "uri";

        public string ConfigurationUri { get; protected set; }

        public void Validate()
        {
            _log.Info("Validating arguments");
            var uri = new Uri(ConfigurationUri);

            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Uri is not absolute");
            }

            if (uri.IsFile)
            {
                throw new ArgumentException("Uri is a file");
            }

            if (!uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Uri is not http nor https");
            }

            if (uri.HostNameType == UriHostNameType.Unknown)
            {
                throw new ArgumentException("Uri's host is of an unknown type");
            }

            _log.Info("Validation OK");
        }
    }
}