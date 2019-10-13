using System.Threading.Tasks;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;

namespace UptimeHippoApi.UptimeHandler.Services.Monitoring
{
    internal interface IMonitoringService
    {
        Task Monitor(IMonitorsRepository monitorsRepository, IMonitorLogsRepository monitorLogRepository);
    }
}