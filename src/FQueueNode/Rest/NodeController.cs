using FQueue;
using Microsoft.AspNetCore.Mvc;
using FQueue.Health;
using FQueue.Rest;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueueNode.Rest
{
    /// <summary>
    /// Default RandomSimulation controller.
    /// </summary>
    [Route(Engine.FQUEUE + "/" + NODE)]
    public class NodeController : FQueueController, INodeController
    {
#warning TODO - unit tests
#warning TODO - swagger adnotations
#warning TODO - każda z operacji zwraca ustalony kod błędu, że "Backup Pending" jak backup trwa

        public const string NODE = "node";
        public const string METHOD_DEQUEUE = "dequeue";
        public const string METHOD_COUNT = "count";
        public const string METHOD_PEEK = "peek";
        public const string METHOD_TAG = "tag";
        public const string METHOD_ENQUEUE = "enqueue";
        public const string METHOD_BACKUP = "backup";

        public NodeController(IHealthChecker healthChecker)
            : base(healthChecker)
        {
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_DEQUEUE)]
        [SwaggerOperation()]
        public string Dequeue([FromRoute][SwaggerParameter()] string queueName, [FromQuery][SwaggerParameter()] int count)
        {
            if (count < 1)
            {
                count = 1;
            }

#warning TODO
            return null;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_COUNT)]
        public int Count([FromRoute] string queueName)
        {
#warning TODO
            return 0;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_PEEK)]
        public string Peek([FromRoute] string queueName)
        {
#warning TODO
            return null;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_PEEK + "/" + METHOD_TAG)]
        public string PeekTag([FromRoute] string queueName)
        {
#warning TODO
            return null;
        }

        [HttpPost(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_ENQUEUE)]
        public StatusCodeResult Enqueue([FromRoute] string queueName, [FromBody] string entry)
        {
#warning TODO
            return null;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_BACKUP)]
        public string Backup([FromRoute] string queueName, [FromQuery] string filename)
        {
#warning TODO - jak bez nazwy pliku, to standardowa nazwa - zwraca ścieżkę, gdzie backup został zrobiony
            return null;
        }
    }
}