namespace FQueue.Watchdog.Watchers
{
    public interface IWatcher
    {
        string Name { get; }
        bool Check();
    }
}