using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;
using UptimeHippoApi.UptimeHandler.Services.Monitoring;
using UptimeHippoApi.UptimeHandler.Services.PushNotification;

namespace UptimeHippoApi.UptimeHandler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
           .AddTransient<IMonitorsRepository, MonitorsRepository>()
           .AddTransient<IMonitorLogsRepository, MonitorLogsRepository>()
           .AddTransient<IMonitoringService, MonitoringService>()
           .AddTransient<IPushNotificationService, PushNotificationService>()
           .AddTransient<IPushNotificationTokensRepository, PushNotificationTokensRepository>()
           .BuildServiceProvider();

            var monitorsRepository = serviceProvider.GetService<IMonitorsRepository>();
            var monitoringService = serviceProvider.GetService<IMonitoringService>();
            var monitorLogsRepository = serviceProvider.GetService<IMonitorLogsRepository>();
            var pushNotificationService = serviceProvider.GetService<IPushNotificationService>();
            var pushNotificationTokensRepository = serviceProvider.GetService<IPushNotificationTokensRepository>();

            Task.Run(async () =>
            {
                await UptimeMonitor.Run(monitorsRepository,
                    monitorLogsRepository,
                    monitoringService,
                    pushNotificationService,
                    pushNotificationTokensRepository);
            }).GetAwaiter().GetResult();
        }
    }
}