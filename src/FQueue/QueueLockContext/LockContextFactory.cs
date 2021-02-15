﻿using FQueue.Context;

namespace FQueue.QueueLockContext
{
    public class LockContextFactory : ILockContextFactory
    {
#warning TODO - unit tests

        private readonly IQueueContextFactory _queueContextFactory;

        public LockContextFactory(IQueueContextFactory queueContextFactory)
        {
            _queueContextFactory = queueContextFactory;
        }

        public ILockContext CreateLockContext(string queueName)
        {
            return new FileLockContext(_queueContextFactory.GetContext(queueName));
        }
    }
}