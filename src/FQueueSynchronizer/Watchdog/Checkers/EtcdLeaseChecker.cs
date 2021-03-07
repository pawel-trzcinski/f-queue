using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FQueue.Configuration;
using FQueueSynchronizer.Etcd;
using log4net;

namespace FQueueSynchronizer.Watchdog.Checkers
{
    public class EtcdLeaseChecker : IEtcdLeaseChecker
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(EtcdLeaseChecker));

        private const int TASK_CANCEL_TIMEOUT_S = 5;

        private readonly IEtcdWrapper _etcdWrapper;
        private readonly IServerUri _serverUri;
        private readonly IConfigurationReader _configurationReader;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _keepAliveTask;
        private bool _checkingRunning;

        public long LeaseId { get; private set; }

        public EtcdLeaseChecker(IEtcdWrapper etcdWrapper, IServerUri serverUri, IConfigurationReader configurationReader)
        {
            _etcdWrapper = etcdWrapper;
            _serverUri = serverUri;
            _configurationReader = configurationReader;
        }

        private static long CreateLeaseId()
        {
            Random random = new Random();
            byte[] bytes = Enumerable.Repeat(0, 8).Select(p => Convert.ToByte(random.Next(256))).ToArray();
            return BitConverter.ToInt64(bytes);
        }

        public void StartChecking()
        {
            if (_checkingRunning)
            {
                throw new InvalidOperationException();
            }

            LeaseId = CreateLeaseId();

            if (!_etcdWrapper.CreateLease(_serverUri.Uri, LeaseId, _configurationReader.Configuration.LeaderElection.LeaseTtlS))
            {
                throw new IOException($"Unable to create lease in {_serverUri.Uri} valued {LeaseId}");
            }

            _keepAliveTask = _etcdWrapper.StartKeepAlive(_serverUri.Uri, LeaseId, _cancellationTokenSource.Token);
            _checkingRunning = true;
        }

        public void StopChecking()
        {
            if (!_checkingRunning)
            {
                throw new InvalidOperationException();
            }

            _cancellationTokenSource.Cancel();
            _keepAliveTask.Wait(TimeSpan.FromSeconds(TASK_CANCEL_TIMEOUT_S));

            if (!_keepAliveTask.Wait(TimeSpan.FromSeconds(TASK_CANCEL_TIMEOUT_S)))
            {
                _log.Error($"{nameof(EtcdLeaseChecker)} did not end in allowed time");
            }

            _checkingRunning = false;
        }
    }
}