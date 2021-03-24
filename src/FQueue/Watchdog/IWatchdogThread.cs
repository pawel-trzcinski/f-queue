using System;

namespace FQueue.Watchdog
{
    public interface IWatchdogThread
    {
        void StartChecking(Action enableAction, Action disableAction);
        void StopChecking();
    }
}