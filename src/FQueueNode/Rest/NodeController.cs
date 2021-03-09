using System.IO;
using System.Text;
using FQueue;
using Microsoft.AspNetCore.Mvc;
using FQueue.Health;

namespace FQueueNode.Rest
{
    /// <summary>
    /// Default RandomSimulation controller.
    /// </summary>
    [Route(Engine.FQUEUE + "/" + NODE)]
    public class NodeController : Controller, INodeController
    {
        public const string NODE = "node";

#warning TODO - przesyłanie strumieniem jest bez sensu, bo base64 string zajmie 2 razy więcej miejsca => posługujemy się tylko xml i json => strumieniowy protokół tylko w plikach
#warning TODO - operacje czysto tagowe - czyli tak na prawdę zwykły peek i to chyba wsio
        // GET /{queueName}/Dequeue? count = 1
        // GET /{queueName}/Count
        // GET /{queueName}/Peek
        // POST /{queueName}/Enqueue
        // GET /{queueName}/Backup?filename={filename} - jak bez nazwy pliku, to standardowa nazwa - zwraca ścieżkę, gdzie backup został zrobiony
#warning TODO - każda z operacji zwraca ustalony kod błędu, że "Backup Pending" jak backup trwa
#warning TODO - unit tests

        private readonly IHealthChecker _healthChecker;

        public NodeController(IHealthChecker healthChecker)
        {
            this._healthChecker = healthChecker;
        }

        /// <summary>
        /// Method used for debugging as well as for health-check actions.
        /// </summary>
        /// <returns>Always 200.</returns>
        [HttpGet("test")]
        public StatusCodeResult Test()
        {
            return this.Ok();
        }

        public class Bin3
        {
            public string Val { get; set; }
            public byte[] BytesCheck { get; set; }
        }

        [HttpGet("bin")]
        public Bin3 GetBin()
        {
            return new Bin3
            {
                Val = "Hello World",
                BytesCheck = new byte[] {5,13,8,23}
            };
        }

        [HttpGet("bins")]
        [Produces("application/octet-stream")]
        public Stream GetBinStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes("Hello World"));
        }

        //[HttpGet("next")]
        //[AddCorsHeader]
        //[AddGitHubHeader]
        //public ContentResult Next()
        //{
        //    int result = this._valueCalculator.GetInt32(this._randomBytesPuller.Pull(4));
        //    _historyStorage.StoreNext(result);
        //    return Content(result.ToString(CultureInfo.InvariantCulture), TEXT_PLAIN);
        //}

        //[HttpGet("next/{max}")]
        //[AddCorsHeader]
        //[AddGitHubHeader]
        //public ContentResult Next([FromRoute] int max)
        //{
        //    int result = this._valueCalculator.GetInt32(this._randomBytesPuller.Pull(4), max);
        //    _historyStorage.StoreNextMax(result);
        //    return Content(result.ToString(CultureInfo.InvariantCulture), TEXT_PLAIN);
        //}

        //[HttpGet("next/{min}/{max}")]
        //[AddCorsHeader]
        //[AddGitHubHeader]
        //public ContentResult Next([FromRoute]int min, [FromRoute]int max)
        //{
        //    int result = this._valueCalculator.GetInt32(this._randomBytesPuller.Pull(4), min, max);
        //    _historyStorage.StoreNextMinMax(result);
        //    return Content(result.ToString(CultureInfo.InvariantCulture), TEXT_PLAIN);
        //}

        //[HttpGet("next-double")]
        //[AddCorsHeader]
        //[AddGitHubHeader]
        //public ContentResult NextDouble()
        //{
        //    double result = this._valueCalculator.GetDouble(this._randomBytesPuller.Pull(8));
        //    _historyStorage.StoreNextDouble(result);
        //    return Content(result.ToString("G17", CultureInfo.InvariantCulture), TEXT_PLAIN);
        //}

        //[HttpGet("next-bytes/{count:range(1,50)}")]
        //[AddCorsHeader]
        //[AddGitHubHeader]
        //public ContentResult NextBytes([FromRoute]int count)
        //{
        //    byte[] result = this._randomBytesPuller.Pull(count);
        //    _historyStorage.StoreNextBytes(result);
        //    return Content(Convert.ToBase64String(result), TEXT_PLAIN);
        //}

        //[HttpGet("histogram/{ranges:range(5,200)}")]
        //[AddCorsHeader]
        //[AddGitHubHeader]
        //public ContentResult Histogram(int ranges)
        //{
        //    return Content(_historyStorage.GetHistogramReport(ranges), TEXT_PLAIN);
        //}

        [HttpGet("health")]
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