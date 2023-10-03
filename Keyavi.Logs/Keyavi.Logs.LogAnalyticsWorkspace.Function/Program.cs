using Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;
using Keyavi.Logs.LogAnalyticsWorkspace.Function.LogAnalyticsWorkspace;
using Keyavi.Logs.LogAnalyticsWorkspace.Function.Options;
using Keyavi.Logs.LogAnalyticsWorkspace.Function.Services;
using Keyavi.Logs.Sdk.Configuration;
using Keyavi.Logs.Sdk.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Keyavi.Logs.Sdk;
using Keyavi.Logs.LogAnalyticsWorkspace.Function;

void ConfigureServices(IServiceCollection services)
{

    services
            .AddOptions<KeyaviApiOptions>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("KeyaviApi").Bind(settings);
        });

    services
              .AddOptions<AzureLogWorkspaceOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("AzureLogWorkspace").Bind(settings);
            });

    services
           .AddOptions<AzureStorageAccountOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("AzureStorageAccount").Bind(settings);
            });

    // Normal AddLogging
    services.AddLogging(configure =>
    {
        configure.SetMinimumLevel(LogLevel.Debug);

    });

    // Additional code to register the ILogger as a ILogger<T> where T is the Startup class
    services.AddSingleton(typeof(ILogger), typeof(Logger<Program>));

    services.AddSingleton<IDataCollector<List<FullPayloadLog>>, SentinelDataCollector<List<FullPayloadLog>>>();
    services.AddSingleton<ITimeSnapshotService, BlobStorageTimeSnapshotService>();
    services.AddSingleton<ILogSyncService, PullFromKeyaviApiLogSyncService>();
    services.AddSingleton<IKeyaviAuditApiClient>(provider =>
    {
        var options = provider.GetService<IOptions<KeyaviApiOptions>>();
        var keyaviApiConfiguration = new KeyaviApiConfiguration(options.Value.BaseUrl, options.Value.ClientId, options.Value.ClientSecret, options.Value.TenantId, options.Value.Scopes, options.Value.IsB2C, options.Value.Authority);
        return AuthenticationProviderExtensions.BuildKeyaviAuditApiClient(keyaviApiConfiguration);
    });
    services.AddHttpClient();
}

var host = new HostBuilder()

  .ConfigureFunctionsWorkerDefaults()
  .ConfigureAppConfiguration(c =>
  {
      c.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
      c.AddEnvironmentVariables();
      c.AddUserSecrets<Program>();
  })
  .ConfigureServices(ConfigureServices)
  .Build();

await host.RunAsync();