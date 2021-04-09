using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class QueueNotFoundResult<T> : ExecutorResult<T>
    {
        public QueueNotFoundResult()
            : base(QueueNotFoundResponseAttribute.CODE, default)
        {
        }
    }

    public class QueueNotFoundResult : ExecutorResult
    {
        public QueueNotFoundResult()
            : base(QueueNotFoundResponseAttribute.CODE)
        {
        }
    }
}