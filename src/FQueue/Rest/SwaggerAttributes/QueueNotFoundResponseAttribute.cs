using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class QueueNotFoundResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public QueueNotFoundResponseAttribute()
            : base((int) HttpStatusCode.NotFound)
        {
            Description = "Queue of the given name was not found.";
        }
    }
}