using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class RejectedByThrottlingResponseAttribute : SwaggerResponseAttribute
    {
        private const HttpStatusCode CODE = HttpStatusCode.TooManyRequests;

        public RejectedByThrottlingResponseAttribute()
            : base((int) CODE)
        {
            Description = "FQueue REST throttling rejected the call.";
        }
    }
}