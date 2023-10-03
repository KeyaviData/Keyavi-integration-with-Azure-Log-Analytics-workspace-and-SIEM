using Refit;

namespace Keyavi.Logs.Sdk.Core;

public class GetFullPayloadLogsQueryParams
{
    [AliasAs("page")]
    public int Page { get; set; }

    [AliasAs("pageSize")]
    public int PageSize { get; set; }

    [AliasAs("startTime")]
    public string StartTime { get; set; }

    [AliasAs("endTime")]
    public string EndTime { get; set; }

    [AliasAs("sort")]
    public string Sort { get; set; }
}