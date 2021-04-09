using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class QueueDeadResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public const HttpStatusCode CODE = HttpStatusCode.Gone;

        public QueueDeadResponseAttribute()
            : base((int)CODE)
        {
            Description = "The queue is in a **dead** state.";
        }
    }
}