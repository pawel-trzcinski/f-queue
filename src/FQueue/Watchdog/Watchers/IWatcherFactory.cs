namespace FQueue.Watchdog.Watchers
{
    public interface IWatcherFactory
    {
        IWatcher CreateWatcher<T>() where T : class, IWatcher;
    }
}