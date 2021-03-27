using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FQueue.Watchdog.Checkers;
using log4net;

namespace FQueue.Watchdog
{
    public abstract class WatchdogThread : IWatchdogThread
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(WatchdogThread));

        private const int TASK_CANCEL_TIMEOUT_S = 10;
        private const int CHECK_INTERVAL_MS = 5000;
        protected static int _checkIntervalMs = CHECK_INTERVAL_MS;

        private Task _checkTask;

        private Action _enableAction;
        private Action _disableAction;
        private bool _logicRunning;

        private bool _watchdogRunning;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        protected readonly Func<IEnumerable<IChecker>> _getCheckers;

        protected WatchdogThread(Func<IEnumerable<IChecker>> getCheckers)
        {
            _getCheckers = getCheckers;
        }

        public void StartChecking(Action enableAction, Action disableAction)
        {
            if (_watchdogRunning)
            {
                throw new InvalidOperationException();
            }

            _log.Info("Starting watchdog");

            _enableAction = enableAction;
            _disableAction = disableAction;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            StartSpecificBackgroundTasks();
            _checkTask = Task.Run(CheckInLoop, _cancellationToken);

            _watchdogRunning = true;
        }

        public void StopChecking()
        {
            if (!_watchdogRunning)
            {
                throw new InvalidOperationException();
            }

            _log.Info("Stopping watchdog");

            StopSpecificBackgroundTasks();

            _cancellationTokenSource.Cancel();
            if (!_checkTask.Wait(TimeSpan.FromSeconds(TASK_CANCEL_TIMEOUT_S)))
            {
                _log.Error("Checker task did not end in allowed time");
            }
        }

        protected virtual void StartSpecificBackgroundTasks()
        {
        }

        protected virtual void StopSpecificBackgroundTasks()
        {
        }

        private void CheckInLoop()
        {
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    bool allWatchersResult = true;

                    foreach (IChecker watcher in _getCheckers())
                    {
                        allWatchersResult = CheckSingleWatcher(watcher);

                        if (!allWatchersResult)
                        {
                            _log.Info($"Watcher {watcher.Name} is not ok");
                            break;
                        }
                    }

                    if (allWatchersResult && !_logicRunning)
                    {
                        _log.Info("All watchers ok. Starting logic.");
                        _enableAction();
                        _logicRunning = true;
                    }

                    if (!allWatchersResult && _logicRunning)
                    {
                        _log.Warn("Not all watchers are ok. Stopping logic.");
                        _disableAction();
                        _logicRunning = false;
                    }

                    _cancellationToken.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(_checkIntervalMs));
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }

        private static bool CheckSingleWatcher(IChecker watcher)
        {
            try
            {
                _log.Info($"Checking {watcher.Name}");
                return watcher.Check();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return false;
            }
        }
    }
}