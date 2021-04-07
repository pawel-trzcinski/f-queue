using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class RejectedByThrottlingResponseAttribute : SwaggerResponseAttribute
    {
#warning TODO - unit tests
        public RejectedByThrottlingResponseAttribute()
            : base((int)HttpStatusCode.TooManyRequests)
        {
            Description = "FQueue REST throttling rejected the call.";
        }
    }
}