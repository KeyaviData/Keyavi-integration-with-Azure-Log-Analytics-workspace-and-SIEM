namespace Keyavi.Logs.Sdk.Core;

public class PayloadLog
{
    public long LogId { get; set; }
    public Guid UserId { get; set; }
    public Guid PayloadId { get; set; }
    public string PayloadName { get; set; }
    public Guid CurrentPayloadOwnerId { get; set; }
    public string ActionAttempted { get; set; }
    public string Result { get; set; }
    public string ResultReason { get; set; }
    public DateTime LogTimestamp { get; set; }
    public UserNetwork UserNetwork { get; set; }

    public string OId { get; set; }
    public string OIdProviderName { get; set; }
}