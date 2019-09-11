using Newtonsoft.Json;

namespace UptimeHippoApi.Data.Models.Authentication
{
    public class Token
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("expiryDate")]
        public System.DateTime ExpiryDate { get; set; }
    }
}