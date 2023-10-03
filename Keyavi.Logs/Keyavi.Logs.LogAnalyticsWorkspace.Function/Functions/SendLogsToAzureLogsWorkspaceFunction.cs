using Keyavi.Logs.LogAnalyticsWorkspace.Function.Core;
using Microsoft.Azure.Functions.Worker;


namespace Keyavi.Logs.LogAnalyticsWorkspace.Function.Functions

{
    public class SendLogsToAzureLogsWorkspaceFunction
    {
        private readonly ILogSyncService _logSyncService;

        public SendLogsToAzureLogsWorkspaceFunction(ILogSyncService logSyncService)
        {
            _logSyncService = logSyncService ?? throw new ArgumentNullException(nameof(logSyncService));
        }

        [Function(nameof(SendLogsToAzureLogsWorkspaceFunction))]
        public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] MyInfo timerInfo,
            FunctionContext context)
        {
            await _logSyncService.SyncAsync();
        }


        public class MyInfo
        {
            public MyScheduleStatus ScheduleStatus { get; set; }

            public bool IsPastDue { get; set; }
        }

        public class MyScheduleStatus
        {
            public DateTime Last { get; set; }

            public DateTime Next { get; set; }

            public DateTime LastUpdated { get; set; }
        }
    }
}