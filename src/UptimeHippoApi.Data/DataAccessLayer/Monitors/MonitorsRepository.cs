using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UptimeHippoApi.Data.DataContext;
using UptimeHippoApi.Data.Models.WebResource;

namespace UptimeHippoApi.Data.DataAccessLayer.Monitors
{
    public class MonitorsRepository : BaseRepository, IMonitorsRepository
    {
        public async Task CreateMonitor(Monitor monitor)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                DataContext.Monitors.Add(monitor);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task DeactivateMonitor(Monitor monitor)
        {
            monitor.Active = false;
            await UpdateMonitor(monitor);
        }

        public async Task<Monitor> FindMonitor(Guid monitorId)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                var monitor
                     = await DataContext.Monitors
                         .Where(m => m.Id == monitorId)
                             .Include(m => m.ApplicationUser)
                                 .FirstOrDefaultAsync();
                return monitor;
            }
        }

        public async Task<IEnumerable<Monitor>> GetMonitorsByUser(IdentityUser user)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                var monitors =
                    await DataContext.Monitors
                        .Where(monitor => monitor.ApplicationUserId == user.Id)
                            .OrderByDescending(monitor => monitor.DateCreated)
                                .ToListAsync();
                return monitors;
            }
        }

        public async Task UpdateMonitor(Monitor monitor)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                DataContext.Monitors.Update(monitor);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}