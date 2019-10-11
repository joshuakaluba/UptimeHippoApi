using Newtonsoft.Json;
using UptimeHippoApi.Data.Models.Domain.Entity;

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

        [JsonProperty("keyWord")]
        public string KeyWord { get; set; }

        [JsonProperty("port")]
        public string Port { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}