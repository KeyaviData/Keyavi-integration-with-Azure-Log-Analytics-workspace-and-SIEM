using Refit;

namespace Keyavi.Logs.Sdk.Core;
public interface IKeyaviAuditApiClient
{
    [Get("/api/logs/full-payload")]
    Task<PagedResult<FullPayloadLog>> GetFullPayloadLogsAsync(GetFullPayloadLogsQueryParams queryParams);
}