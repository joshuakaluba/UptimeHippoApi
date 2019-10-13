using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.UptimeHandler.Services.Monitoring;

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
           .BuildServiceProvider();

            var monitorsRepository = serviceProvider.GetService<IMonitorsRepository>();
            var monitoringService = serviceProvider.GetService<IMonitoringService>();

            Task.Run(async () =>
            {
                await monitoringService.Monitor(monitorsRepository);
            }).GetAwaiter().GetResult();
        }
    }
}