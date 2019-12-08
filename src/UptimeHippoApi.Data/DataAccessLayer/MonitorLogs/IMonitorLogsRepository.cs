using System.Collections.Generic;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.DataAccessLayer.MonitorLogs
{
    public interface IMonitorLogsRepository
    {
        Task SaveMonitorLogs(IEnumerable<MonitorLog> monitorLogs);

        Task<IEnumerable<MonitorLog>> GetMonitorLogsByMonitor(Monitor monitor);
    }
}