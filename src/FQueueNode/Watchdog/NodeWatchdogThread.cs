using FQueue.Watchdog;
using FQueue.Watchdog.Watchers;

namespace FQueueNode.Watchdog
{
    public class NodeWatchdogThread : WatchdogThread
    {
#warning TODO - unit tests

        public NodeWatchdogThread(IWatcherFactory watcherFactory)
            : base(() => new IWatcher[] {watcherFactory.CreateWatcher<IDiskSpaceWatcher>()})
        {
        }
    }
}