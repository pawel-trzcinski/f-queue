using System;
using System.IO;
using log4net;

namespace FQueue.Watchdog.Checkers
{
    public class DiskSpaceChecker : IDiskSpaceChecker
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(DiskSpaceChecker));

        private const long TWENTY_MEGABYTES = 20 * 1024 * 1024;

        public string Name => nameof(DiskSpaceChecker);

        public bool Check()
        {
            var driveInfo = new DriveInfo(Environment.CurrentDirectory);

            if (driveInfo.AvailableFreeSpace >= TWENTY_MEGABYTES)
            {
                return true;
            }

            _log.Error("Available disk free space is less that 20MB");
            return false;
        }
    }
}