using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class QueueDeadResult<T> : ExecutorResult<T>
    {
        public QueueDeadResult()
            : base(QueueDeadResponseAttribute.CODE, default)
        {
        }
    }

    public class QueueDeadResult : ExecutorResult
    {
        public QueueDeadResult()
            : base(QueueDeadResponseAttribute.CODE)
        {
        }
    }
}