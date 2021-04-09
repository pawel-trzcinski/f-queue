using FQueue.Rest.SwaggerAttributes;

namespace FQueue.Logic
{
    public class MaintenancePendingResult<T> : ExecutorResult<T>
    {
        public MaintenancePendingResult()
            : base(MaintenancePendingResponseAttribute.CODE, default)
        {
        }
    }

    public class MaintenancePendingResult : ExecutorResult
    {
        public MaintenancePendingResult()
            : base(MaintenancePendingResponseAttribute.CODE)
        {
        }
    }
}