namespace Keyavi.Logs.Sdk.Core;

public class FullPayloadLog : PayloadLog
{
    public string UserLastNameFirstName { get; set; }
    public string UserEmailAddress { get; set; }
    public string CurrentOwnerLastNameFirstName { get; set; }
    public string CurrentOwnerEmailAddress { get; set; }
}