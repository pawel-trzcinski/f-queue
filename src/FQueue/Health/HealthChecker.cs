using System.Collections.Generic;
using System.Linq;

namespace FQueue.Health
{
#warning TODO - unit tests
    public class HealthChecker : IHealthChecker
    {
        private readonly List<IHealthProvider> _providers = new List<IHealthProvider>(50);

        public void Register(IHealthProvider healthProvider)
        {
#warning TODO - zarejestrować to z różnych (albo jednej) klas
            _providers.Add(healthProvider);
        }

        public HealthStatus GetHealthStatus()
        {
            bool[] aliveStatuses = _providers.Select(p => p.IsAlive).ToArray();

            int aliveCount = aliveStatuses.Count(p => p);

            if (aliveCount == aliveStatuses.Length)
            {
                return HealthStatus.Healthy;
            }

            if (aliveCount > aliveStatuses.Length / 2)
            {
                return HealthStatus.SoSo;
            }

            if (aliveStatuses.Length == 0)
            {
                return HealthStatus.Dead;
            }

            if (aliveCount == 0)
            {
                return HealthStatus.Dead;
            }

            return HealthStatus.AlmostDead;
        }
    }
}