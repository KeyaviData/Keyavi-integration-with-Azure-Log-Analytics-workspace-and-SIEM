namespace Keyavi.Logs.Sdk;

using Keyavi.Logs.Sdk.Configuration;
using Keyavi.Logs.Sdk.Core;
using Keyavi.Logs.Sdk.Middleware;
using Microsoft.Identity.Client;
using Refit;

public static class AuthenticationProviderExtensions
{
    public static async Task<AuthenticationResult> AuthenticateAsync(this KeyaviApiConfiguration options)
    {
        var builder = ConfidentialClientApplicationBuilder.Create(options.ClientId)
            .WithTenantId(options.TenantId)
            .WithClientSecret(options.ClientSecret);

        if (options.IsB2C)
        {
            builder = builder.WithB2CAuthority(options.Authority);
        }

        var confidentialClientApplication = builder
            .Build();

        return await confidentialClientApplication.AcquireTokenForClient(options.Scopes).ExecuteAsync();
    }

    public static IKeyaviAuditApiClient BuildKeyaviAuditApiClient(this KeyaviApiConfiguration options)
    {
        return RestService.For<IKeyaviAuditApiClient>(new HttpClient(new AuthHeaderHandler(options))
        {
            BaseAddress = new Uri(options.BaseUrl)
        });
    }
}
