using SimpleInjector;

namespace FQueue.Watchdog.Watchers
{
    public class WatcherFactory : IWatcherFactory
    {
#warning TODO - unit tests
        private readonly Container _container;

        public WatcherFactory(Container container)
        {
            _container = container;
        }

        public IWatcher CreateWatcher<T>() where T : class, IWatcher
        {
            return _container.GetInstance<T>();
        }
    }
}