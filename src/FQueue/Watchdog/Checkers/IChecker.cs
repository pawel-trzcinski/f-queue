namespace FQueue.Watchdog.Checkers
{
    /// <summary>
    /// Llogic that executes once (and fast if possible) and reports if execution was successfull.
    /// It can be used for periodic disk space checking or leader election checking.
    /// </summary>
    public interface IChecker
    {
        string Name { get; }
        bool Check();
    }
}