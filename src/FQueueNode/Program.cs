using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using CommandLine;
using FQueue.Settings;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace FQueueNode
{
    // Operacje:
    // - Peek - super szybka
    // - Enqueue(1 albo wiele)
    // - Dequeue(1 albo wiele)
    // - Count - super szybka
    // 
    // API:
    // GET /{queueName}/Dequeue? count = 1
    // GET /{queueName}/Count
    // GET /{queueName}/Peek
    // POST /{queueName}/Enqueue
    // GET /{queueName}/Backup?filename={filename} - jak bez nazwy pliku, to standardowa nazwa - zwraca ścieżkę, gdzie backup został zrobiony
    // 
#warning TODO - kontroler ma path "fqueue"
#warning TODO - każda z operacji zwraca ustalony kod błędu, że "Backup Pending" jak backup trwa
    // 
    // 
    // Protokół:
    // Powtarzaj l razy:
    // 1B - tag length = n
    // nB - tag
    // 2B - msg length = k
    // kB - msg - read to end

#warning TODO - nie operujemy wyjątkami
#warning TODO - nazwa kolejki: duże i małe litery, cyfry, minus, podkreślenie
#warning TODO - generator xlsx, który pokazuje czasy każdej operacji - do porównywania czy zmiany, które zrobiliśmy poprawiają wydajność czy nie
#warning TODO - ograniczenie ilości elementów trzymanych w pamięci
#warning TODO - ograniczenie pamięci zużywanej przez proces
#warning TODO - ograniczenie wielkości pojedyńczego pliku bazy - minimum 50MB, maximum 
#warning TODO - w każdym repozytorium jest plik z wersją (Guid;czas UTC;bieżący plik bazy; bieżący wskaźnik w pliku) node przy każdej operacji czyta plik z wersją i porównuje; jak różne, to resetuje bufor w pamięci
#warning TODO - enqueue i dequeue zmienia wersję
#warning TODO - bufor w pamięci dopełnia się w tle po każdym dequeue
#warning TODO - zrobić jakoś, żeby to wszystko działo się strumieniami, żeby nie było za dużo przepisywania pamięci (może dane odzielnie trzymać w RAM a dane operacyjne, wskaźniki itp. oddizelnie? )
#warning TODO - każdy element bufora w pamięci posiada nazwę pliku i wskaźnik w pliku na jaki należy przestawić biezący wskaźnik o operacji dequeue tego elementu
#warning TODO - struktura plików (100 plików w folderze nazwanych 00-99; foldery wewnętrzne o nazwach 00-99; foldery zewnętrzne o nazwach 00000000-99999999 - 8 cyfr; nazwa kolejki )
#warning TODO - backup wszystkiego
#warning TODO - REST throttling
#warning TODO - file handler ma odpalonych na stałe tyle workerów ile jest repozytoriów (żeby nie tworzyć wielu wątków za każdym razem)
#warning TODO - file handler odpowiada za synchroniczny update plików danych i pliku wersji - jakiś command pattern z możliwością rollbacku
#warning TODO - cykliczne, niezależne porównywanie (jakaś synchronizacja z FileHandler - a może to kolejna operacja FileHandler?) spójności danych we repo (klika harmonogramów: {kiedy + moc sprawdzania})
#warning TODO - każde wykrycie braku synchronizacji - wywalenie serwisu
    public static class Program
    {
        private static ILog _log = LogManager.GetLogger(typeof(Program));

        public static int Main(string[] args)
        {
            ConfigureLog4Net();

            _log.Info("Starting FQueueSynchronizerHost");

            try
            {
                return ParseArgumentsAndExecute(args);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
                return -1;
            }
        }

        private static void ConfigureLog4Net()
        {
            string folder = Thread.GetDomain().BaseDirectory;
            const string FILENAME = "log4net.config";
            string fullPath = Path.Combine(folder, FILENAME);

            ILoggerRepository loggerRepository = GetLoggerRepository();
            XmlConfigurator.Configure(loggerRepository, new FileInfo(fullPath));

            _log = LogManager.GetLogger(typeof(Program));

            _log.Debug("Reading configuration from file " + fullPath);
        }

        private static ILoggerRepository GetLoggerRepository()
        {
            var entryAssembly = Assembly.GetCallingAssembly();
            ILoggerRepository repository = LogManager.GetRepository(entryAssembly);
            return repository;
        }

        private static int ParseArgumentsAndExecute(string[] args)
        {
            ParserResult<CommandLineArguments> parseResult = Parser.Default.ParseArguments<CommandLineArguments>(args);

            //_container = ConfigureInjectionContainer(parseResult.TypeInfo.Current);
            //_container.GetInstance<IDateTimeService>().RegisterApplicationStartTime();

            _log.Info($"Parsing program arguments: {String.Join(" ", args)}");
            return parseResult.MapResult
            (
                (CommandLineArguments commandLineArguments) =>
                {
                    // to wszystko poniżej powinien engine robić itp
#warning TODO - zbadaj spójność danych i czy we wszystkich repozytoriach jest to samo (np to co w pliku wersji musi się odnosić do realnej sytuacji na dysku)
#warning TODO - konfigurowalna moc sprawdzania spójności przy starcie (same pliki wersji, struktura katalogów i plików, wsie dane - w kilku wątkach porównujemy pliki)
#warning TODO - startuj wątki i poczekaj aż się wsio odpali
#warning TODO - odpal hosta REST
                    _log.Debug("list verb selected");

                    //_container.GetInstance<IListEngine>().Execute(CommandLineOptions.FromCommandLineArguments(commandLineArguments));

                    return 0;
                },
                notParsedErrors =>
                {
                    HandleParseErrors(notParsedErrors.GetEnumerator());
                    return -1;
                }
            );
        }

        private static void HandleParseErrors(IEnumerator<Error> errorsEnumerator)
        {
            var sb = new StringBuilder();
            sb.Append("Parse errors detected:");
            while (errorsEnumerator.MoveNext())
            {
                Error error = errorsEnumerator.Current;
                if (error == null)
                {
                    throw new ArgumentException(sb.ToString());
                }

                if (error.Tag == ErrorType.HelpRequestedError || error.Tag == ErrorType.HelpVerbRequestedError)
                {
                    // help argument detected; just return - everything else will be taken care of
                    return;
                }

                sb.Append(errorsEnumerator.Current?.Tag);
            }

            throw new ArgumentException(sb.ToString());
        }
    }
}
