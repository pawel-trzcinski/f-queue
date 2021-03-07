namespace FQueue.Watchdog.Checkers
{
    public interface IChecker
    {
        void StartChecking();
        void StopChecking();
    }
}