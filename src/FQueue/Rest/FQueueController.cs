using System;
using FQueue.Health;
using FQueue.Logic;
using FQueue.Rest.HeaderAttributes;
using FQueue.Rest.SwaggerAttributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest
{
    [RejectedByThrottlingResponse]
    public abstract class FQueueController : Controller, IFQueueController
    {
#warning TODO - unit tests
        public const string QUEUE_NAME = "queueName";
        public const string QUEUE_NAME_PATH_TEMPLATE = "{" + QUEUE_NAME + "}";

        private readonly IHealthChecker _healthChecker;

        public ControllerContext Context
        {
            set => ControllerContext = value;
        }

        protected FQueueController(IHealthChecker healthChecker)
        {
            this._healthChecker = healthChecker;
        }

        protected string ConsumeResult(LogicResult result)
        {
            HttpContext.Response.StatusCode = (int) result.Status;
            return result.IsOk && result.ShouldHaveData ? result.DataToString() : String.Empty;
        }

        /// <summary>
        /// Method used for debugging as well as for health-check actions.
        /// </summary>
        /// <returns>Always 200.</returns>
        [HttpGet("test")]
        [AddCorsHeader]
        [AddGitHubHeader]
        [SwaggerOperation(Summary = "Test method that does nothing.")]
        [SuccessResponse]
        public StatusCodeResult Test()
        {
            return this.Ok();
        }

        [HttpGet("health")]
        [AddCorsHeader]
        [AddGitHubHeader]
#warning TODO - swagger attributes po tym jak już zdefiniujemy dokładnie co tu zwraca
        public IActionResult Health()
        {
            HealthStatus healthStatus = _healthChecker.GetHealthStatus();

            if (healthStatus == HealthStatus.Dead)
            {
                return StatusCode(500);
            }

            return Content(((int) healthStatus).ToString());
        }
    }
}