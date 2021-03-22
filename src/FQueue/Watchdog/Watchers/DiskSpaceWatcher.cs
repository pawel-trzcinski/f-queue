using System;
using System.IO;
using log4net;

namespace FQueue.Watchdog.Watchers
{
    public class DiskSpaceWatcher : IDiskSpaceWatcher
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(DiskSpaceWatcher));

        private const long TWENTY_MEGABYTES = 20 * 1024 * 1024;

        public string Name => nameof(DiskSpaceWatcher);

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