using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UptimeHippoApi.Data.DataContext;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.DataAccessLayer.MonitorLogs
{
    public sealed class MonitorLogsRepository : BaseRepository, IMonitorLogsRepository
    {
        public async Task<IEnumerable<MonitorLog>> GetMonitorLogsByMonitor(Monitor monitor)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                var monitorLogs
                    = await DataContext.MonitorLogs
                    .Where(m => m.MonitorId == monitor.Id)
                    .OrderByDescending(m => m.DateCreated)
                    .Include(m => m.Monitor)
                    .Take(500)
                    .ToListAsync();

                return monitorLogs;
            }
        }

        public async Task SaveMonitorLogs(IEnumerable<MonitorLog> monitorLogs)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                foreach (var monitorLog in monitorLogs)
                {
                    DataContext.MonitorLogs.Add(monitorLog);
                }

                await DataContext.SaveChangesAsync();
            }
        }
    }
}