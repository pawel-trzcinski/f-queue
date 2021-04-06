using FQueue.Health;
using FQueue.Rest.HeaderAttributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest
{
    public abstract class FQueueController : Controller, IFQueueController
    {
#warning TODO - unit tests
        public const string QUEUE_NAME = "queueName";
        public const string QUEUE_NAME_PATH_TEMPLATE = "{" + QUEUE_NAME + "}";

        private readonly IHealthChecker _healthChecker;

        protected FQueueController(IHealthChecker healthChecker)
        {
            this._healthChecker = healthChecker;
        }

        /// <summary>
        /// Method used for debugging as well as for health-check actions.
        /// </summary>
        /// <returns>Always 200.</returns>
        [HttpGet("test")]
        [AddCorsHeader]
        [AddGitHubHeader]
        [SwaggerOperation]
        public StatusCodeResult Test()
        {
            return this.Ok();
        }

        [HttpGet("health")]
        [AddCorsHeader]
        [AddGitHubHeader]
        public IActionResult Health()
        {
            HealthStatus healthStatus = _healthChecker.GetHealthStatus();

            if (healthStatus == HealthStatus.Dead)
            {
                return StatusCode(500);
            }

            return Content(((int)healthStatus).ToString());
        }
    }
}