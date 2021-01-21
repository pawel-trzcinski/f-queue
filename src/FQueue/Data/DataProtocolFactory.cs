using System;
using System.Linq;
using SimpleInjector;

namespace FQueue.Data
{
    public class DataProtocolFactory : IDataProtocolFactory
    {
#warning TODO - unit tests

        private readonly Container _container;

        public DataProtocolFactory(Container container)
        {
            _container = container;
        }

        public IDataProtocol GetProtocol(DataProtocolVersion dataProtocolVersion)
        {
#warning TEST
            IDataProtocol protocol = _container.GetAllInstances<IDataProtocol>().SingleOrDefault(p => p.Version == dataProtocolVersion);

            if (protocol == null)
            {
                // this exception means error in implementation => If we have DataProtocolVersion enum value, we should have protocol class registered
                throw new InvalidOperationException($"No protocol {dataProtocolVersion} registered in container");
            }

            return protocol;
        }
    }
}