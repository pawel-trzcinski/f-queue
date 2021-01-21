namespace FQueue.Data
{
    public enum DataProtocolVersion : byte
    {
#warning TODO - unit test, że jak się ma różne wersje protokołu w pliku, to poprawnie czyta
        None = 0,

        V01BasicProtocol = 1
    }
}