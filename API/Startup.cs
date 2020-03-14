using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MassTransit;
using MassTransit.Definition;
using MassTransit.OpenTracing;
using OpenTracing.Util;
using Shared;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddOpenTracing();
            services.AddSingleton(serviceProvider =>
            {
                var config = Jaeger.Configuration.FromEnv(serviceProvider.GetRequiredService<ILoggerFactory>());

                var tracer = config.GetTracer();
                GlobalTracer.Register(tracer);

                return tracer;
            });
            
            services.AddMassTransit(x =>
            {

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

                x.AddRequestClient<FibonacciRequestV1>();
            });

            services.AddHostedService<BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}