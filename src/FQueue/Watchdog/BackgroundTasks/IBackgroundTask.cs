namespace FQueue.Watchdog.BackgroundTasks
{
    /// <summary>
    /// Routine that is started and runs in the background.
    /// Used to run LeaderElection KeepAlive task for example.
    /// </summary>
    public interface IBackgroundTask
    {
        void Start();
        void Stop();
    }
}