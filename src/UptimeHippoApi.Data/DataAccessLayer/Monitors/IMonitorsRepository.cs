using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.DataAccessLayer.Monitors
{
    public interface IMonitorsRepository
    {
        Task CreateMonitor(Monitor monitor);

        Task DeleteMonitor(Monitor monitor);

        Task<Monitor> FindMonitor(Guid id);

        Task UpdateMonitor(Monitor monitor);

        Task<IEnumerable<Monitor>> GetMonitorsByUser(IdentityUser user);
    }
}