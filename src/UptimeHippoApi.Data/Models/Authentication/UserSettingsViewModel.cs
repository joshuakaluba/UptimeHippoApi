using Newtonsoft.Json;

namespace UptimeHippoApi.Data.Models.Authentication
{
    public class UserSettingsViewModel
    {
        public UserSettingsViewModel()
        {
        }

        public UserSettingsViewModel(ApplicationUser user)
        {
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            NotificationTextMessagesEnabled = user.NotificationTextMessagesEnabled;
            NotificationEmailsEnabled = user.NotificationEmailsEnabled;
            PushNotificationsEnabled = user.PushNotificationsEnabled;
        }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("notificationTextMessagesEnabled")]
        public bool NotificationTextMessagesEnabled { get; set; }

        [JsonProperty("notificationEmailsEnabled")]
        public bool NotificationEmailsEnabled { get; set; }

        [JsonProperty("pushNotificationsEnabled")]
        public bool PushNotificationsEnabled { get; set; }
    }
}