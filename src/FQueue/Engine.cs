using System.Net;
using System.Reflection;
using FQueue.Configuration;
using FQueue.Rest.Throttling.Middlewares;
using FQueue.Watchdog;
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
    public abstract class Engine : IEngine
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Engine));

        public const string FQUEUE = "fqueue";
        public const string FQUEUE_CAPITALIZED = "FQueue";

        private readonly IWatchdogThread _watchdogThread;
        private readonly IControllerFactory _controllerFactory;

        private IWebHost _webHost;

        protected Engine(IWatchdogThread watchdogThread, IControllerFactory controllerFactory)
        {
            _watchdogThread = watchdogThread;
            _controllerFactory = controllerFactory;
        }

        protected abstract Assembly GetControllerAssembly();

        public void Start()
        {
            _watchdogThread.StartChecking
            (
                () =>
                {
                    _log.Info("Starting engine specific logic");
                    StartSpecificLogic();

                    _log.Info("Starting engine hosting");
                    StartHosting();
                },
                () =>
                {
                    _log.Info("Stopping engine specific logic");
                    StopSpecificLogic();

                    _log.Info("Stopping engine hosting");
                    StopHosting();
                }
            );
        }

        public void Stop()
        {
            _watchdogThread.StopChecking();
        }

        protected abstract void StartSpecificLogic();
        protected abstract void StopSpecificLogic();

        protected abstract RestConfiguration GetRestConfiguration();

        private void StartHosting()
        {
            RestConfiguration restConfiguration = GetRestConfiguration();

            _webHost = WebHost.CreateDefaultBuilder(null)
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                    options.Limits.MaxConcurrentConnections = restConfiguration.Throttling.MaximumServerConnections;

                    options.Listen(IPAddress.Any, restConfiguration.HostingPort); //http://0.0.0.0:7081
                })
                .ConfigureServices(services =>
                {
                    services.AddLogging();
      
                    services.AddMvc(o =>
                    {
                        //o.EnableEndpointRouting = false;

                        o.RespectBrowserAcceptHeader = true;

                        o.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    }).AddApplicationPart(GetControllerAssembly());
                    
                    services.AddSingleton(_controllerFactory);

                    services.AddSwaggerGen(c =>
                    {
                        c.EnableAnnotations();
                        c.SwaggerDoc(FQUEUE, new OpenApiInfo {Title = FQUEUE_CAPITALIZED, Version = "v1"});
                    });
                })
                .Configure(app =>
                {
                    app.UseMiddleware<ThrottlingMiddleware>(restConfiguration.Throttling);
                    app.UseRouting();
                    app.UseSwagger();

                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint($"/swagger/{FQUEUE}/swagger.json", FQUEUE_CAPITALIZED);
                    });

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                })
                .Build();

            _webHost.RunAsync();
        }

        private void StopHosting()
        {
            _webHost.StopAsync().Wait();
            _log.Info("Hosting stopped");
        }
    }
}