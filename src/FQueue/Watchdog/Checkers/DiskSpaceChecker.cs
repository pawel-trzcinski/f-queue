using System;
using System.IO;
using log4net;

namespace FQueue.Watchdog.Checkers
{
    public class DiskSpaceChecker : IDiskSpaceChecker
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DiskSpaceChecker));

        public const long TWENTY_MEGABYTES = 20 * 1024 * 1024;

        public string Name => nameof(DiskSpaceChecker);

        protected virtual long GetAvailableFreeSpace()
        {
            return new DriveInfo(Environment.CurrentDirectory).AvailableFreeSpace;
        }

        public bool Check()
        {
            if (GetAvailableFreeSpace() >= TWENTY_MEGABYTES)
            {
                return true;
            }

            _log.Error("Available disk free space is less that 20MB");
            return false;
        }
    }
}