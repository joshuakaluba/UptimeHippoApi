using Newtonsoft.Json;

namespace UptimeHippoApi.UptimeHandler.Models
{
    public class PushMessage
    {
        public PushMessage()
        {
        }

        public PushMessage(string title, string body, string to)
        {
            Title = title;
            Body = body;
            To = to;
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}