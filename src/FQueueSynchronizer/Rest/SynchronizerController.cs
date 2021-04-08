using FQueue;
using Microsoft.AspNetCore.Mvc;
using FQueue.Health;
using FQueue.Rest;

namespace FQueueSynchronizer.Rest
{
    /// <summary>
    /// Default RandomSimulation controller.
    /// </summary>
    [Route(Engine.FQUEUE + "/" + SYNC)]
    public class SynchronizerController : FQueueController, ISynchronizerController
    {
        public const string SYNC = "sync";

#warning TODO - unit tests

#warning TODO - methods

        public SynchronizerController(IHealthChecker healthChecker)
            : base(healthChecker)
        {
        }

    }
}