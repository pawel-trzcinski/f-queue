using FQueue.Watchdog.BackgroundTasks;

namespace FQueueSynchronizer.Watchdog.BackgroundTasks
{
    public interface IEtcdLeaseBackgroundTask : IBackgroundTask
    {
        long LeaseId { get; }
    }
}