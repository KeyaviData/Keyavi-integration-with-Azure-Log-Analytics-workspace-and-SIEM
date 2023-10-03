using System.Net.Http.Headers;
using Keyavi.Logs.Sdk.Configuration;

namespace Keyavi.Logs.Sdk.Middleware;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly KeyaviApiConfiguration _authenticationProviderOptions;

    public AuthHeaderHandler(KeyaviApiConfiguration authenticationProviderOptions)
    {
        _authenticationProviderOptions = authenticationProviderOptions ?? throw new ArgumentNullException(nameof(authenticationProviderOptions));
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authResult = await _authenticationProviderOptions.AuthenticateAsync();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}