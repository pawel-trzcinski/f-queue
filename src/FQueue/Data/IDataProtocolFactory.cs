namespace FQueue.Data
{
    public interface IDataProtocolFactory
    {
        IDataProtocol GetProtocol(DataProtocolVersion dataProtocolVersion);
    }
}