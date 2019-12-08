using System.Collections.Generic;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.Containers
{
    public class ApplicationUserMonitors : Dictionary<ApplicationUser, List<Monitor>>
    {
        public void Add(Monitor monitor)
        {
            if (this.ContainsKey(monitor.ApplicationUser))
            {
                this[monitor.ApplicationUser].Add(monitor);
            }
            else
            {
                var monitorList = new List<Monitor>
                {
                    monitor
                };

                this.Add(monitor.ApplicationUser, monitorList);
            }
        }
    }
}