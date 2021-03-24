namespace FQueue.Watchdog.Checkers
{
    public interface ICheckerFactory
    {
        IChecker CreateChecker<T>() where T : class, IChecker;
    }
}