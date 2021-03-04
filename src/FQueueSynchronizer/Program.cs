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
using log4net;
using log4net.Config;
using log4net.Repository;

namespace FQueueSynchronizer
{
    public static class Program
    {
        private static ILog _log = LogManager.GetLogger(typeof(Program));

        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private static readonly TimeSpan _closingTimeout = TimeSpan.FromSeconds(10);
        private static Task<int> _mainTask;
        private static IEngine _engine;

        private const string ETCD_SERVER_URI = "http://127.0.0.1:2379";

        public static int Main(string[] args)
        {
#if(DEBUG)
            args = new[] {"-u", "http://127.0.0.1/"};
#endif

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

        private static int ParseArgumentsAndExecute(string[] args)
        {
            ParserResult<SynchronizerArguments> parseResult = Parser.Default.ParseArguments<SynchronizerArguments>(args);

            _log.Info($"Parsing program arguments: {String.Join(" ", args)}");
            return parseResult.MapResult
            (
                synchronizerArguments =>
                {
                    synchronizerArguments.Validate();

                    _log.Info("Initializing injection container");
                    _engine = SynchronizerContainerRegistrator
                        .Register()
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

                _engine?.Stop();

                _log.Debug("Waiting for main task to end");
                _mainTask?.Wait(_closingTimeout);
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