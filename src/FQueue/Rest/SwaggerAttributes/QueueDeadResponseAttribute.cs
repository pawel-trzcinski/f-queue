using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class QueueDeadResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public QueueDeadResponseAttribute()
            : base((int)HttpStatusCode.Gone)
        {
            Description = "The queue is in a **dead** state.";
        }
    }
}