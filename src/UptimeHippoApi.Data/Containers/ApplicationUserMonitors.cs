using System.Collections.Generic;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.Containers
{
    public class ApplicationUserMonitors
    {
        public Dictionary<ApplicationUser, List<Monitor>> userMonitors;

        public ApplicationUserMonitors()
        {
            userMonitors = new Dictionary<ApplicationUser, List<Monitor>>();
        }

        public void Add(Monitor monitor)
        {
            if (monitor.Triggered) { return; }

            if (userMonitors.ContainsKey(monitor.ApplicationUser))
            {
                userMonitors[monitor.ApplicationUser].Add(monitor);
            }
            else
            {
                var monitorList = new List<Monitor>();
                monitorList.Add(monitor);

                userMonitors.Add(monitor.ApplicationUser, monitorList);
            }
        }
    }
}