using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class RejectedByThrottlingResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public const HttpStatusCode CODE = HttpStatusCode.TooManyRequests;

        public RejectedByThrottlingResponseAttribute()
            : base((int) CODE)
        {
            Description = "FQueue REST throttling rejected the call.";
        }
    }
}