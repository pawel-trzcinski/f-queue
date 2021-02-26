namespace FQueue.Configuration.Validation
{
    public interface IFQueueConfigurationValidator
    {
        void Validate(FQueueConfiguration configuration);
    }
}