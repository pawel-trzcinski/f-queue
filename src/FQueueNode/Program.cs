using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using FQueue;
using FQueue.Settings;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace FQueueNode
{
#warning TODO - w każdym repozytorium jest plik z wersją (Guid;czas UTC;bieżący plik bazy; bieżący wskaźnik w pliku; ostatni plik bazy; wskaźnik ostatniego elementu) node przy każdej operacji czyta plik z wersją i porównuje pierwszy element; jak różne, to resetuje bufor w pamięci
#warning TODO - w każdej kolejce jest plik, który mówi jaka jest struktura katalogów (ile cyfr w plikach, w wew. katalogach i zew. katalogach); mówi też o nazie choć nie musi; są to wartości niezmienne tworzone w czasie tworzenia kolejki;
#warning TODO - FE ma w pamięci info o wszystkich kolejkach
#warning TODO - BE ma file watchera na każdym pliku info o kolejce. Jak się cokolwiek zmieni w nim w czasie działąnia BE, to znaczy, że ktoś źle zrobił ręcznie i jest FATA error i wsio zatrzymujemy
#warning TODO - FE tylko i wyłącznie robią proste operacje na kolejkach - peek, enqueue i dequeue
#warning TODO - BE służą do synchronizacji dostępu do kolejek

#warning TODO - leader election - ETCD.  HealthCheck (liveness - działa; readiness - jest liderem i wsio zainicjalizowane)
#warning TODO - cała konfiguracja (łącznie z konfiguracją logowania) siedziw etcd w JSON pod jednym kluczem
#warning TODO - BE nasłuchuje zmian konfiguracji i je wdraża na żywo - i powiadamia FE, który też je wdraża
#warning TODO - tylko BE czyta ETCD 
#warning TODO - konfiguracja jest serwowana do FE przez BE
#warning TODO - dockerfiles - self contained
#warning TODO - nie operujemy wyjątkami
#warning TODO - nazwa kolejki: duże i małe litery, cyfry, minus, podkreślenie
#warning TODO - generator xlsx, który pokazuje czasy każdej operacji - do porównywania czy zmiany, które zrobiliśmy poprawiają wydajność czy nie
#warning TODO - ograniczenie ilości elementów trzymanych w pamięci
#warning TODO - ograniczenie pamięci zużywanej przez proces
#warning TODO - enqueue i dequeue zmienia wersję
#warning TODO - bufor w pamięci dopełnia się w tle po każdym dequeue
#warning TODO - zrobić jakoś, żeby to wszystko działo się strumieniami, żeby nie było za dużo przepisywania pamięci (może dane odzielnie trzymać w RAM a dane operacyjne, wskaźniki itp. oddizelnie? )
#warning TODO - każdy element bufora w pamięci posiada nazwę pliku i wskaźnik w pliku na jaki należy przestawić biezący wskaźnik o operacji dequeue tego elementu
#warning TODO - struktura plików (100 plików w folderze nazwanych 00-99; foldery wewnętrzne o nazwach 00-99; foldery zewnętrzne o nazwach 00000000-99999999 - 8 cyfr; nazwa kolejki )
#warning TODO - ustawiamy początkowe capacity na 1mln. Jak kolejka osiągnie połowę capacity, to w tle zwiększamy capacity razy 2 - czyli dodajemy wsie foldery
#warning TODO - jak kolejka jest pusta, to wymuszamy czyszczenie starych plików i zaczynamy od zera
#warning TODO - niech kolejka się zawija, da się ogarnać ring index
#warning TODO - backup wszystkiego
#warning TODO - file handler ma odpalonych na stałe tyle workerów ile jest repozytoriów (żeby nie tworzyć wielu wątków za każdym razem)
#warning TODO - file handler odpowiada za synchroniczny update plików danych i pliku wersji - jakiś command pattern z możliwością rollbacku
#warning TODO - cykliczne, niezależne porównywanie (jakaś synchronizacja z FileHandler - a może to kolejna operacja FileHandler?) spójności danych we repo (klika harmonogramów: {kiedy + moc sprawdzania})
#warning TODO - każde wykrycie braku synchronizacji - przestawienie starszego repozytorium w tryb "synchronizing" (albo wywalamy w pierwotnej wersji) - jak możliwe; (jak ta sama wersja ale nie sync, to wywalamy serwis)

#warning TODO - sprawdzanie bazy danych, też sprawdza wsie CRC32

#warning TODO - oddzielny kontroler dla synchronizacji danych
    // albo raczej synchronizacja to by była taka, że:
    //  - dodajemy na gorąco kolejny folder i mówimy, że ma status "synchronizing"
    //  - w tle dociąga on sobie pliki, których nie ma i kasuje te które już są stare
    //  - kiedy algorytm uzna, że już mu bardzo mało zostało, to zatrzymuje cały system (jak przy operacji na kolejce) i synchronizuje się ostateczne i zmienia status na "active"
    //  - a ten wątek, co w tle chodzi i sprawdza synchronizacje, to też może folder przestawić w status synchronizing
    //  - 
#warning TODO - REST method - Version. Jak się synchronizują, to tylko z taką samą wersją
#warning TODO - REST method - stan repozytoriów - lista i czy są aktywne czy się synchronizują czy może są kaput
    public static class Program
    {
        private static ILog _log = LogManager.GetLogger(typeof(Program));

        public const string DEFAULT_CONFIGURATION_FILENAME = "./appsettings.json";

        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private static readonly TimeSpan _closingTimeout = TimeSpan.FromSeconds(10);
        private static Task<int> _mainTask;
        private static IEngine _engine;

        public static int Main(string[] args)
        {
#warning TODO - only and always one argument - BE address

            ConfigureLog4Net();

            _log.Info("Starting FQueueNode");

            try 
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                AssemblyLoadContext.Default.Unloading += Default_Unloading;

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

            _log.Info($"Parsing program arguments: {String.Join(" ", args)}");
            return parseResult.MapResult
            (
                commandLineArguments =>
                {
                    _log.Info("Initializing injection container");
                    _engine = ContainerRegistrator
                        .Register(String.IsNullOrWhiteSpace(commandLineArguments.ConfigurationFilePath) ? DEFAULT_CONFIGURATION_FILENAME : commandLineArguments.ConfigurationFilePath)
                        .GetInstance<IEngine>();

                    _mainTask = Task.Run(() =>
                    {
                        _engine.Start();
                        return 0;
                    });

                    Console.CancelKeyPress += OnExit;
                    _closing.WaitOne();

                    return _mainTask.Result;
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

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            _log.Debug(nameof(Default_Unloading));
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                _log.Info("Process exiting");

                _engine.Stop();

                _log.Debug("Waiting for main task to end");
                _mainTask.Wait(_closingTimeout);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
            finally
            {
                _log.Debug("All is finished");
            }
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            _log.Info("Exit invoked");
            _closing.Set();
        }

    }
}