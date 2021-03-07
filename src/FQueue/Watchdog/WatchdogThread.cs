using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FQueue.Watchdog.Watchers;
using log4net;

namespace FQueue.Watchdog
{
    public abstract class WatchdogThread : IWatchdogThread
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(WatchdogThread));

        private const int TASK_CANCEL_TIMEOUT_S = 10;
        private const int CHECK_INTERVAL_S = 5;

        private Task _checkTask;

        private Action _startAction;
        private Action _endAction;
        private bool _logicRunning;

        private bool _watchdogRunning;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        private readonly IEnumerable<IWatcher> _watchers;

        protected WatchdogThread(IEnumerable<IWatcher> watchers)
        {
            _watchers = watchers;
        }

        public void StartChecking(Action startAction, Action endAction)
        {
            if (_watchdogRunning)
            {
               throw new InvalidOperationException();
            }

            _log.Info("Starting watchdog");

            _startAction = startAction;
            _endAction = endAction;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            StartSpecificCheckers();
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

            StopSpecificCheckers();

            _cancellationTokenSource.Cancel();
            if (!_checkTask.Wait(TimeSpan.FromSeconds(TASK_CANCEL_TIMEOUT_S)))
            {
                _log.Error("Checker task did not end in allowed time");
            }
        }

        protected virtual void StartSpecificCheckers()
        {
        }

        protected virtual void StopSpecificCheckers()
        {
        }

        private void CheckInLoop()
        {
            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    bool allWatchersResult = true;

                    foreach (IWatcher watcher in _watchers)
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
                        _startAction();
                        _logicRunning = true;
                    }
                    
                    if(!allWatchersResult && _logicRunning)
                    {
                        _log.Warn("Not all watchers are ok. Stopping logic.");
                        _endAction();
                        _logicRunning = false;
                    }

                    _cancellationToken.WaitHandle.WaitOne(TimeSpan.FromSeconds(CHECK_INTERVAL_S));
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }

        private static bool CheckSingleWatcher(IWatcher watcher)
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