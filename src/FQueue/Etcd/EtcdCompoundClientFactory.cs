using dotnet_etcd;

namespace FQueue.Etcd
{
    public class EtcdCompoundClientFactory : IEtcdCompoundClientFactory
    {
        public IEtcdCompoundClient CreateClient(string serverUri)
        {
            EtcdClient client = new EtcdClient(serverUri);
            return new EtcdCompoundClient(client, client);
        }
    }
}