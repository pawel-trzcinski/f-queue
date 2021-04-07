using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class MaintenancePendingResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public MaintenancePendingResponseAttribute()
            : base((int)HttpStatusCode.ServiceUnavailable)
        {
            Description = "Maintenance operation is pending for the queue. No operation is possible for the queue. Returned only if **InternalQueueOperationReturnsError** is set. Otherwise call is blocked until maintenance operation is completed.";
        }
    }
}