using Newtonsoft.Json;

namespace UptimeHippoApi.Common.Exception
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;
        }

        public ErrorMessage(System.Exception exception)
        {
            Message = exception.Message;
        }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}