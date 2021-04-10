using Swashbuckle.AspNetCore.Annotations;

namespace FQueue.Rest.SwaggerAttributes
{
    public class QueueNameParameterAttribute : SwaggerParameterAttribute
    {
        private const string QUEUE_NAME_DESCRIPTION = "Name of the queue operation is going to be executed on.";

        public QueueNameParameterAttribute()
        {
            Description = QUEUE_NAME_DESCRIPTION;
            Required = true;
        }
    }
}