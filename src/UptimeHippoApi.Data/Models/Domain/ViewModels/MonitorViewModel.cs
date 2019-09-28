using Newtonsoft.Json;
using UptimeHippoApi.Data.Models.Domain.Entity;
using UptimeHippoApi.Data.Models.WebResource;

namespace UptimeHippoApi.Data.Models.Domain.ViewModels
{
    public class MonitorViewModel
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("type")]
        public MonitorTypeEnum Type { get; set; }

        [JsonProperty("interval")]
        public MonitorIntervalEnum Interval { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;
    }
}