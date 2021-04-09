namespace FQueueNode.Logic
{
    public class NodeQueueManager : INodeQueueManager
    {
#warning TODO - lokalny lock po nazwie kolejki - nie ma sensu pytać BE skoro już tutaj jest blokada
#warning TODO - tutaj jest łądowanie kolejki w tle z BE do cache
#warning TODO - operacje robią tak, że ile tylko się da wybierają z cache i powiadamiają BE (do rozpropagowania), że zrobiła się operacja
#warning TODO - tutaj ciągniemy dane z cache jakby co


        //każda operacja:
        //  lokalny lock na queueName
        //    Jak queue dead, to wychodzimy - to powinno być propagowane automatycznie - przez BE
        //    Jak queue maintenance, to wychodzimy - to powinno być propagowane automatycznie - przez BE (jakiś long polling ?)
        //    BE.TryGetLock(queueName, bool createQueueIfNotExists) // tu wyjdzie, że backup albo, że nie ma queue - automatyczne stworzenie ścieżki robi propagację do nodeów
        //    jak nie OK, to zwraca kod błędu (backupPending, maintenance)
    }
}

#warning TODO - każdy Node rejestruje się w BE. BE śle informacje synchronizacyjne (nowe kolejki, statusy kolejek; WERSJE KOLEJEK - to mówi o tym, że wersja się zmieniła)
