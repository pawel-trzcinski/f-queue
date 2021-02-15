using System;
using FQueue.Context;

namespace FQueue.QueueLockContext
{
    /// <summary>
    /// The purpose of LockContext is to create a scope where application has exclusive lock on a specific queue
    /// </summary>
    public interface ILockContext : IDisposable
    {
        QueueContext Context { get; }
    }
}