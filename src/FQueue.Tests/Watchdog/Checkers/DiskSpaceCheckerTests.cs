using FQueue.Watchdog.Checkers;
using NUnit.Framework;

namespace FQueue.Tests.Watchdog.Checkers
{
    [TestFixture]
    public class DiskSpaceCheckerTests
    {
        private class DiskSpaceCheckerTester : DiskSpaceChecker
        {
            private readonly long _availableFreeSpace;

            public DiskSpaceCheckerTester(long availableFreeSpace)
            {
                _availableFreeSpace = availableFreeSpace;
            }

            protected override long GetAvailableFreeSpace()
            {
                return _availableFreeSpace;
            }
        }

        [TestCase(DiskSpaceChecker.TWENTY_MEGABYTES - 10, false)]
        [TestCase(DiskSpaceChecker.TWENTY_MEGABYTES - 1, false)]
        [TestCase(DiskSpaceChecker.TWENTY_MEGABYTES, true)]
        [TestCase(DiskSpaceChecker.TWENTY_MEGABYTES + 1, true)]
        [TestCase(DiskSpaceChecker.TWENTY_MEGABYTES + 10, true)]
        public void CheckerExecutes(long availableFreeSpace, bool result)
        {
            var tester = new DiskSpaceCheckerTester(availableFreeSpace);

            Assert.IsNotNull(tester.Name);
            Assert.AreEqual(result, tester.Check());
        }
    }
}