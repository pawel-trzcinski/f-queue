using SimpleInjector;

namespace FQueue.Watchdog.Checkers
{
    public class CheckerFactory : ICheckerFactory
    {
        private readonly Container _container;

        public CheckerFactory(Container container)
        {
            _container = container;
        }

        public IChecker CreateChecker<T>() where T : class, IChecker
        {
            return _container.GetInstance<T>();
        }
    }
}