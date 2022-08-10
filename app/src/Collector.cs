namespace sample;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class Collector : BackgroundService
{
    public class Props
    {
        public string? text { get; init; }
    }
    private readonly Props _props;

    public static Collector FromConfig(IConfiguration config)
    {
        var props = new Props
        {
            text = config["TEXT"],
        };
        return new Collector(props);
    }

    public Collector(Props props)
    {
        _props = props;
    }

    public void HelloWorld(CancellationToken cancellationToken, string text)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Sample Project: {text}");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // func
        // loop
        // Task.Run(() => HelloWorld(stoppingToken, _props.text));
        return Task.CompletedTask;
    }


    // BackgroundService는 생략가능
    // public override Task StopAsync(CancellationToken cancellationToken)
    // {
    //     return Task.CompletedTask;
    // }
}
