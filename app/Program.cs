using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using dotenv.net;


using sample;
using GeneralProducer.Handlers;
using GeneralConsumer.Handlers;

DotEnv.Load();

using IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();

        if (context.HostingEnvironment.IsProduction())
        {

        }
        else
        {

        }

        services
            // .AddSingleton<>(provider =>
            // {

            // })
            .AddHttpClient()
            .AddHostedService<Producer>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                return Producer.FromConfig(config);
            })
            .AddHostedService<Consumer>()
            .AddHostedService<Collector>(provider =>
            {
                // .env
                var config = provider.GetRequiredService<IConfiguration>();
                return Collector.FromConfig(config);
            });
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();
