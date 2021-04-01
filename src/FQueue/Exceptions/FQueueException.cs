using System;
using FQueue.Context;
using JetBrains.Annotations;

namespace FQueue.Exceptions
{
    [Serializable]
    public abstract class FQueueException : Exception
    {
        public QueueContext Context { get; }

        protected FQueueException([NotNull] QueueContext context, string message, Exception innerException = null) 
            : base(message, innerException)
        {
            Context = context;
        }

        public override string ToString()
        {
            return $"{base.ToString()}{Environment.NewLine}{Context}";
        }
    }
}