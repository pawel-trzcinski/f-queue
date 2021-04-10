using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class MaintenancePendingResponseAttribute : SwaggerResponseAttribute
    {
        public const HttpStatusCode CODE = HttpStatusCode.ServiceUnavailable;

        public MaintenancePendingResponseAttribute()
            : base((int) CODE)
        {
            Description = "Maintenance operation is pending for the queue. No operation is possible for the queue. Returned only if **InternalQueueOperationReturnsError** is set. Otherwise call is blocked until maintenance operation is completed.";
        }
    }
}