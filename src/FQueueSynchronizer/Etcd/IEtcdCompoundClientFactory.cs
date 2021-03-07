namespace FQueueSynchronizer.Etcd
{
    public interface IEtcdCompoundClientFactory
    {
        IEtcdCompoundClient CreateClient(string serverUri);
    }
}