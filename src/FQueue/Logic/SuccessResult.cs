using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class SuccessResult<T> : ExecutorResult<T>
    {
        public SuccessResult(T returnData)
            : base(SuccessResponseAttribute.CODE, returnData)
        {
        }
    }

    public class SuccessResult : ExecutorResult
    {
        public SuccessResult()
            : base(SuccessResponseAttribute.CODE)
        {
        }
    }
}