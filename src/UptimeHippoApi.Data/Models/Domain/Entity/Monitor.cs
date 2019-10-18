using Newtonsoft.Json;
using System;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.Models.Domain.Entity
{
    public class Monitor : Auditable
    {
        public Monitor(string url)
        {
            Url = url;
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("keyWord")]
        public string KeyWord { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; } = 80;

        [JsonProperty("lastMonitorDate")]
        public DateTime LastMonitorDate { get; set; }

        [JsonProperty("lastMonitorSuccess")]
        public bool LastMonitorSuccess { get; set; }

        [JsonProperty("type")]
        public MonitorTypeEnum Type { get; set; }

        [JsonProperty("interval")]
        public MonitorIntervalEnum Interval { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}