using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class QueueDeadResponseAttribute : SwaggerResponseAttribute
    {
        public const HttpStatusCode CODE = HttpStatusCode.Gone;

        public QueueDeadResponseAttribute()
            : base((int) CODE)
        {
            Description = "The queue is in a **dead** state.";
        }
    }
}