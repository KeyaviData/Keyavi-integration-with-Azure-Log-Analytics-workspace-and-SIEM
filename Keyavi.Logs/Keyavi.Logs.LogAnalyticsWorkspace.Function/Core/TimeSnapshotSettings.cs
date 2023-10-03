using System;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Core
{
    public record TimeSnapshotSettings
    {
        public DateTime LastSuccessullySyncedTimeSnapshot { get; set; }
        public long? LastSuccessullySyncedLogId { get; set; }
    }
}
