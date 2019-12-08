using System.Collections.Generic;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Containers;
using UptimeHippoApi.Data.DataAccessLayer.MonitorLogs;
using UptimeHippoApi.Data.DataAccessLayer.Monitors;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.UptimeHandler.Services.Monitoring
{
    public interface IMonitoringService
    {
        Task<List<MonitorLog>> Monitor(IEnumerable<Monitor> sitesToMonitor,
            IMonitorsRepository monitorsRepository,
            IMonitorLogsRepository monitorLogRepository);

        ApplicationUserMonitors GetFailedMonitors();
    }
}