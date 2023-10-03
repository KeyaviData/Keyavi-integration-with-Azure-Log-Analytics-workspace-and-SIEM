namespace Keyavi.Logs.Sdk.Configuration;

public class KeyaviApiConfiguration
{
    public string BaseUrl { get; set; }
    public string ClientId { get; }
    public string ClientSecret { get; }

    public string TenantId { get; }

    public string[] Scopes { get; }

    public bool IsB2C { get; set; }

    public string? Authority { get; set; }

    public KeyaviApiConfiguration(string baseUrl, string clientId, string clientSecret, string tenantId, string[] scopes, bool isB2C = false, string? authority = null)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or whitespace.", nameof(baseUrl));
        }

        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new ArgumentException($"'{nameof(clientId)}' cannot be null or whitespace.", nameof(clientId));
        }

        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new ArgumentException($"'{nameof(clientSecret)}' cannot be null or whitespace.", nameof(clientSecret));
        }

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new ArgumentException($"'{nameof(tenantId)}' cannot be null or whitespace.", nameof(tenantId));
        }
        BaseUrl = baseUrl;
        ClientId = clientId;
        ClientSecret = clientSecret;
        TenantId = tenantId;
        Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
        IsB2C = isB2C;

        if (IsB2C && authority == null)
        {
            throw new ArgumentException($"'{nameof(authority)}' cannot be null or whitespace when IsB2C is true.", nameof(authority));
        }
        Authority = authority;
    }
}
