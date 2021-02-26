namespace FQueue.Configuration.Validation
{
    public interface IRestConfigurationValidator
    {
        void Validate(RestConfiguration configuration);
    }
}