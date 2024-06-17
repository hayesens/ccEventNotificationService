using ccEventNotificationService;

public class Program
{
    public static void Main(string[] args)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            })
            .UseWindowsService()
            .ConfigureLogging((hostContext, logging) => logging.AddEventLog());
}
