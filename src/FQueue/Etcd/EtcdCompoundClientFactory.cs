using dotnet_etcd;

namespace FQueue.Etcd
{
    public class EtcdCompoundClientFactory : IEtcdCompoundClientFactory
    {
#warning TODO - unit tests
        public IEtcdCompoundClient CreateClient(string serverUri)
        {
            EtcdClient client = new EtcdClient(serverUri);
            return new EtcdCompoundClient(client, client);
        }
    }
}