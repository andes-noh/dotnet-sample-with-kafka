using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using dotenv.net;
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
            //kafka consumer
            .AddHostedService<Consumer>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                return Consumer.FromConfig(config);
            })
            //kafka producer

            .AddHostedService<Producer>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                return Producer.FromConfig(config);
            });

        // .AddHostedService<Collector>(provider =>
        // {
        //     // .env
        //     var config = provider.GetRequiredService<IConfiguration>();
        //     return Collector.FromConfig(config);
        // });
    })
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();
