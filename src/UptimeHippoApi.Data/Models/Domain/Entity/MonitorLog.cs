using System;

namespace UptimeHippoApi.Data.Models.Domain.Entity
{
    public class MonitorLog : Auditable
    {
        public int ResponseCode { get; set; }

        public Guid MonitorId { get; set; }

        public virtual Monitor Monitor { get; set; }
    }
}