using FQueue.Context;

namespace FQueue.QueueLockContext
{
    public class FileLockContext : ILockContext
    {
        public QueueContext Context { get; }

        public FileLockContext(QueueContext context)
        {
#warning TODO - create lock file

            Context = context;
        }

        public void Dispose()
        {
#warning TODO - destroy lock file
            throw new System.NotImplementedException();
        }

        
    }
}