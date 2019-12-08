using System.Threading.Tasks;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;
using UptimeHippoApi.UptimeHandler.Services.Monitoring;
using UptimeHippoApi.UptimeHandler.Services.PushNotification;

namespace UptimeHippoApi.UptimeHandler
{
    public class UptimeMonitor
    {
        public static async Task Run(IMonitorsRepository monitorsRepository,
            IMonitorLogsRepository monitorLogsRepository,
            IMonitoringService monitoringService,
            IPushNotificationService pushNotificationService,
            IPushNotificationTokensRepository pushNotificationTokensRepository)
        {
            await monitoringService.Monitor(null, monitorsRepository, monitorLogsRepository);

            var applicationUserMonitors = monitoringService.GetFailedMonitors();

            await pushNotificationService.SendPushNotifications(applicationUserMonitors, pushNotificationTokensRepository);
        }
    }
}