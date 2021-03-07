using System;

namespace FQueue.Watchdog
{
    public interface IWatchdogThread
    {
        void StartChecking(Action startAction, Action endAction);
        void StopChecking();
    }
}