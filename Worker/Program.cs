using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jaeger;
using Jaeger.Samplers;
using MassTransit;
using MassTransit.Clients;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;
using MassTransit.OpenTracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(serviceProvider =>
                    {
                        var config = Configuration.FromEnv(serviceProvider.GetRequiredService<ILoggerFactory>());

                        var tracer = config.GetTracer();
                        GlobalTracer.Register(tracer);

                        return tracer;
                    });
                    services.AddMemoryCache();
                    services.AddOpenTracing();
                    services.AddSingleton<IFibonacciService, LocalFibonacciService>();

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<FibonacciRequestV1Consumer>();
                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            cfg.Host("rabbitmq", configurator =>
                            {
                                configurator.Username("admin");
                                configurator.Password("admin");
                            });

                            cfg.PropagateOpenTracingContext();

                            // or, configure the endpoints by convention
                            cfg.ConfigureEndpoints(provider, new KebabCaseEndpointNameFormatter());
                        }));
                    });
                    services.AddHostedService<BusService>();
                });
    }
}