namespace FQueue.Etcd
{
    public interface IEtcdCompoundClientFactory
    {
        IEtcdCompoundClient CreateClient(string serverUri);
    }
}