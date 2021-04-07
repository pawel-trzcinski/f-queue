using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class BackupPendingResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public BackupPendingResponseAttribute()
            : base((int) HttpStatusCode.Locked)
        {
            Description = "Backup operation is pending for the queue. No operation is possible for the queue. Returned only if **InternalQueueOperationReturnsError** is set. Otherwise call is blocked until backup operation is completed.";
        }
    }
}