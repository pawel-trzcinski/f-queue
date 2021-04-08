using System.IO;
using FQueue;
using Microsoft.AspNetCore.Mvc;
using FQueue.Health;
using FQueue.Models;
using FQueue.Rest;
using FQueue.Rest.SwaggerAttributes;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueueNode.Rest
{
    /// <summary>
    /// Default RandomSimulation controller.
    /// </summary>
    [Route(Engine.FQUEUE + "/" + NODE)]
    [RejectedByThrottlingResponse]
    public class NodeController : FQueueController, INodeController
    {
#warning TODO - unit tests
#warning TODO - unit tests - każdy, znaleziony przez reflekcję SwaggerResponse, musi mieć unikalny kod
        
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
        [Produces(ContentTypes.APPLICATION_JSON)]
        [SwaggerOperation(Summary = "Get one or more elements from the begenning of queue.")]
        [SuccessResponse("Returns JSON array of dequeued elements.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        [QueueNotFoundResponse]
        [QueueDeadResponse]
        public string Dequeue
        (
            [FromRoute] [QueueNameParameter] string queueName,
            [FromQuery] [SwaggerParameter(Description = "Number of elements to dequeue. If *count* is less or equal to *1*, single element is returned. Default value is *1*.", Required = false)]
            int count,
            [FromQuery] [SwaggerParameter(Description = "If set, query will return error when number of requested elements is larger than number of available ones.", Required = false)]
            bool checkCount
        )
        {
            if (count < 1)
            {
                count = 1;
            }

#warning TODO
            return null;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_COUNT)]
        [Produces(ContentTypes.TEXT_PLAIN)]
        [SwaggerOperation(Summary = "Get number of elements in the queue.")]
        [SuccessResponse("Returns number of elements in queue.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        [QueueNotFoundResponse]
        [QueueDeadResponse]
        public string Count([FromRoute] [QueueNameParameter] string queueName)
        {
#warning TODO
            return 0.ToString();
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_PEEK)]
        [Produces(ContentTypes.APPLICATION_JSON)]
        [SwaggerOperation(Summary = "Peek first element in the queue.")]
        [SuccessResponse("Returns first element of the queue without removing it.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        [QueueNotFoundResponse]
        [QueueDeadResponse]
        public string Peek([FromRoute] [QueueNameParameter] string queueName)
        {
#warning TODO
            return null;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_PEEK + "/" + METHOD_TAG)]
        [Produces(ContentTypes.TEXT_PLAIN)]
        [SwaggerOperation(Summary = "Peek Tag string of the firs element in the queue.")]
        [SuccessResponse("Returns Tag string of the first element.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        [QueueNotFoundResponse]
        [QueueDeadResponse]
        public string PeekTag([FromRoute] [QueueNameParameter] string queueName)
        {
#warning TODO
            return null;
        }

        [HttpPost(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_ENQUEUE)]
        [Consumes(ContentTypes.APPLICATION_JSON)]
        [SwaggerOperation(Summary = "Insert elements at the end of queue.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        [SuccessResponse]
        [QueueDeadResponse]
        public StatusCodeResult Enqueue
        (
            [FromRoute] [QueueNameParameter] string queueName,
            [FromBody]
            [SwaggerRequestBody(Description = "JSON object or JSON array of objects that are to be enqueued. Every objects must have **Tag** field defined. Any other JSON structure may be added inside the object.", Required = true)]
            // this parameter is for swagger documentation only
            TagObject entry
        )
        {
            if (HttpContext.Request.Body.CanSeek)
            {
                HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            string entryString;
            using (StreamReader reader = new StreamReader(HttpContext.Request.BodyReader.AsStream()))
            {
                entryString = reader.ReadToEnd();
            }

#warning TODO
            return null;
        }

        [HttpGet(QUEUE_NAME_PATH_TEMPLATE + "/" + METHOD_BACKUP)]
        [Produces(ContentTypes.TEXT_PLAIN)]
        [SwaggerOperation(Summary = "Backup specific queue.")]
        [SuccessResponse("Returns path of the backup file.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        [QueueNotFoundResponse]
        [QueueDeadResponse]
        public string Backup
        (
            [FromRoute] [QueueNameParameter] string queueName,
            [FromQuery] [SwaggerParameter(Description = "File path which queue will be backed up to. The path must be accesible to FQueue node. If *filename* is absent, default one will be used.", Required = false)]
            string filename
        )
        {
#warning TODO - jak bez nazwy pliku, to standardowa nazwa - zwraca ścieżkę, gdzie backup został zrobiony
            return null;
        }

        [HttpGet(METHOD_BACKUP)]
        [Produces(ContentTypes.TEXT_PLAIN)]
        [SwaggerOperation(Summary = "Backup all queues.")]
        [SuccessResponse("Returns folder in which backup files were stored.")]
        [BackupPendingResponse]
        [MaintenancePendingResponse]
        public string BackupAll
        (
            [FromQuery] [SwaggerParameter(Description = "Folder path which all queues will be backed up to. The path must be accesible to FQueue node. If *folder* is absent, default one will be used.", Required = false)]
            string folder
        )
        {
#warning TODO - jak bez nazwy foldery, to standardowa nazwa - zwraca ścieżkę, gdzie backupy zostały zrobione
            return null;
        }
    }
}