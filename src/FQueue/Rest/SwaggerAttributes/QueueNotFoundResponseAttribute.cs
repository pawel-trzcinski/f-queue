using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class QueueNotFoundResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public const HttpStatusCode CODE = HttpStatusCode.NotFound;

        public QueueNotFoundResponseAttribute()
            : base((int) CODE)
        {
            Description = "Queue of the given name was not found.";
        }
    }
}