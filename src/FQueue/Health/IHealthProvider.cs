namespace FQueue.Health
{
    public interface IHealthProvider
    {
        bool IsAlive { get; }
    }
}