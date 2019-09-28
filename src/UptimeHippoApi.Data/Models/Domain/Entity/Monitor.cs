using Newtonsoft.Json;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.Models.WebResource
{
    public class Monitor : Auditable
    {
        public Monitor(string url)
        {
            Url = url;
        }

        [JsonProperty("url")]
        public string Url { get; set; }

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