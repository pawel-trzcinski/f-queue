using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using CommandLine;
using FQueue.Settings;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace FQueueSynchronizerHost
{
    public static class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

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
            XmlConfigurator.ConfigureAndWatch(loggerRepository, new FileInfo(fullPath));

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
            ParserResult<object> parseResult = Parser.Default.ParseArguments<CommandLineArguments>(args);

            _container = ConfigureInjectionContainer(parseResult.TypeInfo.Current);
            _container.GetInstance<IDateTimeService>().RegisterApplicationStartTime();

            _log.Info($"Parsing program arguments: {String.Join(" ", args)}");
            return parseResult.MapResult
            (
                (CommandLineArguments commandLineArguments) =>
                {
                    _log.Debug("list verb selected");

                    _container.GetInstance<IListEngine>().Execute(CommandLineOptions.FromCommandLineArguments(commandLineArguments));

                    // jak  config replica nie jest null, to odpal replica


                    // jak config

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
