using System.Net;

namespace FQueue.Logic
{
    public abstract class ExecutorResult<T>
    {
        public HttpStatusCode Status { get; }

        public T ReturnData { get; }

        protected ExecutorResult(HttpStatusCode status, T returnData)
        {
            Status = status;
            ReturnData = returnData;
        }
    }

    public abstract class ExecutorResult : ExecutorResult<int>
    {
        protected ExecutorResult(HttpStatusCode status)
            : base(status, default)
        {
        }
    }
}