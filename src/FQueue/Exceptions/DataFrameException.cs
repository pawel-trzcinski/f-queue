using System;
using FQueue.Context;
using JetBrains.Annotations;

namespace FQueue.Exceptions
{
    [Serializable]
    public class DataFrameException : FQueueException
    {
        public DataFrameException([NotNull] QueueContext context, string message) 
            : base(context, message)
        {
        }
    }
}