namespace FQueue.Configuration.Validation
{
    public interface IThrottlingConfigurationValidator
    {
        void Validate(ThrottlingConfiguration configuration);
    }
}