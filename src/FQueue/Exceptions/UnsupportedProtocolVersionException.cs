using System;
using FQueue.Context;
using JetBrains.Annotations;

namespace FQueue.Exceptions
{
    [Serializable]
    public class UnsupportedProtocolVersionException : DataFrameException
    {
        public byte UnsupportedProtocol { get; }

        public UnsupportedProtocolVersionException([NotNull] QueueContext context, byte protocolVersion) 
            : base(context, $"Protocol {protocolVersion} is not supported")
        {
            UnsupportedProtocol = protocolVersion;
        }

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}UnsupportedProtocol: {UnsupportedProtocol}";
        }
    }
}