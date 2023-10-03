using Refit;

namespace Keyavi.Logs.Sdk.Core;

public class GetPayloadLogsQueryParams
{
    [AliasAs("page")]
    public int Page { get; set; }

    [AliasAs("pageSize")]
    public int PageSize { get; set; }

}
