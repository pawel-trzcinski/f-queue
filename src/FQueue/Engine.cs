using System;
using System.Net;
using System.Threading;
using FQueue.Health;
using FQueue.Rest.Throttling.Middlewares;
using FQueue.Settings;
using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FQueue
{
    /// <summary>
    /// Main engine of the app. It contains all the bad, non-injectable stuff.
    /// </summary>
    public class Engine : IEngine
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Engine));

        private IWebHost _webHost;

        private readonly IConfigurationReader _configurationReader;
        private readonly IControllerFactory _controllerFactory;
        //private readonly IImageDownloadTaskFactory _imageDownloadTaskFactory;
        //private readonly ITaskMaster _taskMaster;
        //private readonly IRandomBytesPuller _randomBytesPuller;
        private readonly IHealthChecker _healthChecker;

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public Engine
        (
            IConfigurationReader configurationReader,
            IControllerFactory controllerFactory,
            //IImageDownloadTaskFactory imageDownloadTaskFactory,
            //ITaskMaster taskMaster,
            //IRandomBytesPuller randomBytesPuller,
            IHealthChecker healthChecker
        )
        {
            _configurationReader = configurationReader;
            _controllerFactory = controllerFactory;
            //_imageDownloadTaskFactory = imageDownloadTaskFactory;
            //_taskMaster = taskMaster;
            //_randomBytesPuller = randomBytesPuller;
            _healthChecker = healthChecker;
        }

        public void Start()
        {
            _log.Info($"Reporting configuration:{Environment.NewLine}{ _configurationReader.Configuration.ReportConfiguration()}");

#warning TODO - zbadaj spójność danych i czy we wszystkich repozytoriach jest to samo (np to co w pliku wersji musi się odnosić do realnej sytuacji na dysku)
#warning TODO - konfigurowalna moc sprawdzania spójności przy starcie (same pliki wersji, struktura katalogów i plików, wsie dane - w kilku wątkach porównujemy pliki)
#warning TODO - startuj wątki i poczekaj aż się wsio odpali
#warning TODO - odpal hosta REST
            
            _log.Info("Starting data acquisition");
            StartDataAcquisition();

            _log.Info("Starting engine hosting");
            StartHosting();
        }

        public void Stop()
        {
            _log.Info("Stopping data acquisition");
            StopDataAcquisition();

            _log.Info("Stopping engine hosting");
            StopHosting();
        }

        private void StartDataAcquisition()
        {
            //_log.Info($"Creating tasks of count {_configurationReader.Configuration.ImageDownload.FrameGrabUrls.Count}");
          
            ////foreach (string url in _configurationReader.Configuration.ImageDownload.FrameGrabUrls.Take(1))
            //foreach (string url in _configurationReader.Configuration.ImageDownload.FrameGrabUrls)
            //{
            //    ISourceTask sourceTask = _imageDownloadTaskFactory.GetNewTask(url);

            //    _taskMaster.Register(sourceTask);
            //    _randomBytesPuller.Register(sourceTask);
            //    _healthChecker.Register(sourceTask);
            //}

            //_taskMaster.StartTasks(_tokenSource.Token);
        }

        /// <summary>
        /// Start REST service hosting.
        /// </summary>
        protected virtual void StartHosting()
        {
            ThrottlingConfiguration throttlingConfiguration = _configurationReader.Configuration.Rest.Throttling;

            _webHost = WebHost.CreateDefaultBuilder(null)
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    options.Limits.MaxConcurrentConnections = throttlingConfiguration.MaximumServerConnections;
                  
                    options.Listen(IPAddress.Any, _configurationReader.Configuration.Rest.HostingPort); //http://0.0.0.0:7081
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(s => _controllerFactory);
                    services.AddLogging();
                    services.AddMvc(o => 
                    {
                        o.EnableEndpointRouting = false;

                        o.RespectBrowserAcceptHeader = true;

                        o.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    });
                    services.AddSwaggerGen(c => { c.SwaggerDoc("fqueue", new OpenApiInfo { Title = "FQueue", Version = "v1" }); });
                })
                .Configure(app =>
                {
                    app.UseMiddleware<ThrottlingMiddleware>(throttlingConfiguration);
                    app.UseMvc();
                    app.UseSwagger();

                    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/fqueue/swagger.json", "FQueue"); });
                })
                .Build();

            _webHost.Run();
        }

        /// <summary>
        /// Stops REST service hosting.
        /// </summary>
        protected virtual void StopHosting()
        {
            _log.Info("Hosting stopping");
            _webHost.StopAsync().Wait();
            _log.Info("Hosting stopped");
        }

        private void StopDataAcquisition()
        {
            _tokenSource.Cancel();
        }
    }
}