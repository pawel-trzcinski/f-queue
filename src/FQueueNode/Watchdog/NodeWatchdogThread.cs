using FQueue.Watchdog;
using FQueue.Watchdog.Checkers;

namespace FQueueNode.Watchdog
{
    public class NodeWatchdogThread : WatchdogThread
    {
#warning TODO - unit tests

        public NodeWatchdogThread(ICheckerFactory checkerFactory)
            : base(() => new IChecker[] {checkerFactory.CreateChecker<IDiskSpaceChecker>()})
        {
        }
    }
}