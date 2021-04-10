using FQueue.Context;
using FQueue.Logic;
using FQueue.Models;

namespace FQueueNode.Logic
{
    public class NodeQueueManager : INodeQueueManager
    {
#warning TODO - tutaj jest łądowanie kolejki w tle z BE do cache
#warning TODO - operacje robią tak, że ile tylko się da wybierają z cache i powiadamiają BE (do rozpropagowania), że zrobiła się operacja
#warning TODO - tutaj ciągniemy dane z cache jakby co


        //każda operacja:
        //  lokalny lock na queueName
        //    Jak queue dead, to wychodzimy - to powinno być propagowane automatycznie - przez BE
        //    Jak queue maintenance, to wychodzimy - to powinno być propagowane automatycznie - przez BE (jakiś long polling ?)
        //    BE.TryGetLock(queueName, bool createQueueIfNotExists) // tu wyjdzie, że backup albo, że nie ma queue - automatyczne stworzenie ścieżki robi propagację do nodeów
        //    jak nie OK, to zwraca kod błędu (backupPending, maintenance)
        public LogicResult Dequeue(QueueContext context, int count, bool checkCount)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public LogicResult Count(QueueContext context)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public LogicResult Peek(QueueContext context)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public LogicResult PeekTag(QueueContext context)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public LogicResult Enqueue(QueueContext context, QueueEntry[] entries)
        {
#warning TODO
            throw new System.NotImplementedException();
        }

        public LogicResult Backup(QueueContext context, string filename)
        {
#warning TODO
            throw new System.NotImplementedException();
        }
    }
}

#warning TODO - każdy Node rejestruje się w BE. BE śle informacje synchronizacyjne (nowe kolejki, statusy kolejek; WERSJE KOLEJEK - to mówi o tym, że wersja się zmieniła)
